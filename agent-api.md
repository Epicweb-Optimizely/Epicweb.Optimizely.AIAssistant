# Agent API - External Integration Endpoint

## Overview

The **Agent API** provides a standardized REST integration endpoint that allows external assistant engines, agent platforms, or custom automation workflows to interact directly with the Optimizely CMS editor's conversational AI assistant. This enables seamless AI-powered workflows from third-party systems while leveraging all built-in AI capabilities, tools, and context management.

The main endpoint is:
```
POST /api/epicweb/agent/chat
```

This endpoint processes conversational requests from external agents and returns AI-powered responses, supporting:
- **Message composition** from external AI orchestration platforms
- **Function calling** via built-in MCP tools or custom tools
- **Context management** with attached content and chat history
- **Draft-based workflow** where suggested changes are saved as unpublished drafts (subject to CMS approval)

---

## Security Model: CMS Identity & Permissions

**Important:** Each Agent API key is mapped to an Optimizely CMS user account. This means:

- **User-based access control** - Every API request executes under a specific CMS user identity
- **Standard CMS permissions** - Content access follows your existing Optimizely user roles and permissions rules
- **Role management** - Use CMS admin UI to manage agent user roles, access levels, and content restrictions exactly as you would for regular editors
- **Audit trail** - All Agent API activities are tracked under the mapped CMS user identity for compliance and monitoring

**Example:** If you map an agent to the `editor` user account, the agent can only access and modify content that the `editor` user has permission to edit in the CMS. No special API permissions needed—everything goes through standard CMS access control.

---

## Architecture & Core Concepts

### Request/Response Flow

1. **External Agent** sends a chat request with message, context, and optional attachments
2. **Agent API Controller** validates authentication and authorization
3. **AI Assistant Service** processes the request using configured AI provider
4. **Tool Execution** (optional) invokes available MCP tools if `useTools: true`
5. **Response** returns AI-generated response, tool results, or error messages
6. **Session Management** maintains chat history and conversation context in cache

### Key Components

| Component | Purpose |
|-----------|---------|
| **AgentController** | HTTP entry point for chat requests; handles authentication filter and response serialization |
| **AgentApiKeyAuthenticationHandler** | ASP.NET Core authentication scheme validating `X-Api-Key` header |
| **DefaultAgentApiKeyValidator** | Validates API key against configured agents in `appsettings.json` |
| **DefaultAgentCmsUserResolver** | Maps authenticated API key to a CMS identity for content permissions |
| **ChatSessionCacheService** | In-memory cache for session state and chat history across requests |
| **IAgentApiOpenApiSecurityProvider** | Generates OpenAPI documentation with security requirements |

---

## Setup and Installation

### 1. Add Project Reference

Reference `Epicweb.Optimizely.AIAssistant.AgentAPI` in your Optimizely CMS project (Alloy or custom application).

### 2. Register in Startup.cs or Program.cs

```csharp
public void ConfigureServices(IServiceCollection services)
{
	// Register basic AI Assistant services
	services.AddAIAssistant();

	// Enable the Agent API endpoints (built-in API key authentication by default)
	services.AddEpicwebAgentApi();
}
```

**Alternative method name**:
```csharp
services.AddAIAssistantAgentAPI();
```

Both methods are equivalent and enable the Agent API with default built-in API key authentication.

### 3. Configure Route (Optional)

By default, MVC routing exposes the controller under `/api/epicweb/agent/chat`.

To override the Agent API prefix globally, configure `Epicweb:AIAssistant:AgentApi:Path` in `appsettings.json`:

```json
{
  "Epicweb": {
	"AIAssistant": {
	  "AgentApi": {
		"Path": "/my-solution/api/epicweb/agent"
	  }
	}
  }
}
```

This changes the endpoints to:
- `POST /my-solution/api/epicweb/agent/chat`
- `GET /my-solution/api/epicweb/agent/health`
- `GET /my-solution/api/epicweb/agent/openapi.json`

---

## Security & Authentication

The `AgentController` is protected by ASP.NET Core authorization policy `Epicweb.AgentApi`. By default, `AddEpicwebAgentApi()` registers:

- Built-in **API key authentication** (`X-Api-Key` header)
- Automatic mapping of authenticated API keys to **CMS user identities**

You can:
- Override the API key validator (`IAgentApiKeyValidator`) to use custom logic
- Replace the entire authentication scheme (e.g., JWT bearer) without modifying the controller

### Default API Key Configuration

Configure one or more API keys in `appsettings.json` that map to CMS user names:

```json
{
  "Epicweb": {
	"AIAssistant": {
	  "AgentApi": {
		"DefaultAgent": {
		  "ApiKey": "replace-with-long-random-secret",
		  "AgentName": "EpicwebAgent",
		  "CmsUserName": "admin"
		},
		"Agents": {
		  "MyAgent": {
			"ApiKey": "another-secret",
			"CmsUserName": "editor"
		  },
		  "ReadOnlyAgent": {
			"ApiKey": "third-secret",
			"CmsUserName": "viewer"
		  }
		}
	  }
	}
  }
}
```

**Key fields:**

| Field | Required | Description |
|-------|----------|-------------|
| `ApiKey` | ✓ | Long random secret (store securely, consider environment variables) |
| `AgentName` | ✗ | Descriptive name for logging/monitoring (metadata only, not used for auth) |
| `CmsUserName` | ✓ | Maps to existing Optimizely CMS user; determines content access permissions |

**Send API key in requests:**

```http
POST /api/epicweb/agent/chat HTTP/1.1
X-Api-Key: replace-with-long-random-secret
Content-Type: application/json

{
  "text": "Write product description for the homepage",
  "context": { "contentId": "152_12" }
}
```

### JWT Bearer Configuration (Custom Authentication)

To use JWT-based authentication instead of built-in API keys:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;

services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		options.Authority = "https://your-issuer";
		options.Audience = "epicweb-agent-api";
	});

services.AddEpicwebAgentApi(options =>
{
	options.UseBuiltInApiKeyAuthentication = false;
	options.AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
});
```

**Send JWT token in requests:**

```http
Authorization: Bearer <jwt-token>
```

### Custom Authentication Implementation

Implement `IAgentApiKeyValidator` to define your own validation logic:

```csharp
public class CustomApiKeyValidator : IAgentApiKeyValidator
{
	public async Task<AgentApiAuthenticationResult> ValidateAsync(
		string apiKey,
		HttpContext context,
		CancellationToken cancellationToken)
	{
		// Your validation logic here
		// Return success with agent name and CMS user name

		return new AgentApiAuthenticationResult
		{
			Succeeded = true,
			AgentName = "MyAgent",
			CmsUserName = "admin",
			Claims = new[] { /* custom claims */ }
		};
	}
}

// Register in Startup.cs
services.AddScoped<IAgentApiKeyValidator, CustomApiKeyValidator>();
```

---

## Discovery Endpoints

The Agent API exposes two lightweight JSON endpoints:

### Health Check

```http
GET /api/epicweb/agent/health
```

**Response:**
```json
{
  "status": "ok",
  "service": "Epicweb Optimizely AI Assistant Agent API",
  "timestampUtc": "2025-02-13T10:30:45.123Z",
  "endpoints": [
	"/api/epicweb/agent/health",
	"/api/epicweb/agent/openapi.json",
	"/api/epicweb/agent/chat"
  ]
}
```

### OpenAPI Document

```http
GET /api/epicweb/agent/openapi.json
```

Returns a complete OpenAPI 3.0.3 manifest for `/api/epicweb/agent/chat` including:
- Endpoint URL and HTTP method
- Authentication security requirement
- Request body schema with examples
- Response examples and error codes

**Use case:** Import into Postman, Swagger UI, or code-generation tools.

---

## Chat Request/Response

### POST /api/epicweb/agent/chat

**Request Body:**

```json
{
  "text": "Write a product description for our homepage banner",
  "context": {
	"contentId": "152_12",
	"contentType": "PageData"
  },
  "attachedContent": [
	{
	  "contentId": "42_7",
	  "contentType": "ImageData"
	}
  ],
  "chatHistory": [
	{
	  "role": "user",
	  "content": "Earlier context message"
	},
	{
	  "role": "assistant",
	  "content": "Earlier response"
	}
  ],
  "sessionId": "session-abc-123",
  "agentName": "MyExternalAgent",
  "useTools": true,
  "isHtml": false
}
```

**Field Descriptions:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `text` | string | ✓ | User message or prompt for the AI assistant |
| `context` | object | ✗ | Current content context (contentId, contentType, language, etc.) |
| `attachedContent` | array | ✗ | List of content items to provide as context (pages, blocks, images, etc.) |
| `chatHistory` | array | ✗ | Previous conversation exchanges for continuation and context |
| `sessionId` | string | ✗ | Unique identifier for the user's current chat session; enables per-session state tracking |
| `agentName` | string | ✗ | Descriptive identifier for the calling AI agent (metadata only; not used for authentication or CMS impersonation) |
| `useTools` | boolean | ✗ | When `true`, the assistant may invoke available MCP tools to gather context or perform actions. Set to `false` for read-only sessions. Default: `true` |
| `isHtml` | boolean | ✗ | If `true`, the input `text` contains HTML tags and markup context should be preserved |

### Successful Response (200 OK)

```json
{
  "text": "Here's a compelling product description for your homepage banner:\n\nOur premium collection combines cutting-edge design with unmatched quality...",
  "tokens": {
	"input": 120,
	"output": 85
  },
  "tools": [
	{
	  "toolName": "SearchContent",
	  "parameters": {
		"query": "similar products"
	  },
	  "result": "Found 5 related products"
	}
  ],
  "updatedChatHistory": [
	{
	  "role": "user",
	  "content": "Write a product description for our homepage banner"
	},
	{
	  "role": "assistant",
	  "content": "Here's a compelling product description for your homepage banner:\n\nOur premium collection combines cutting-edge design with unmatched quality..."
	}
  ],
  "sessionId": "session-abc-123"
}
```

### Error Response (400, 401, 403, 501)

```json
{
  "error": "request_body_required",
  "detail": "A request body is required."
}
```

**Error codes:**

| Code | Status | Meaning |
|------|--------|---------|
| `request_body_required` | 400 | POST body is missing or empty |
| `service_not_registered` | 501 | `AIAssistantService` not configured in DI container |
| `permission_denied` | 403 | Authenticated user lacks access to requested content |
| `unauthorized` | 401 | Missing or invalid `X-Api-Key` header |

---

## OpenAPI & Swagger Documentation

Depending on your documentation tool, register metadata and security definitions:

### Option A: Swashbuckle (ASP.NET Core 6+)

#### 1. OperationFilter Class

Add this filter to your CMS host project:

```csharp
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class AgentApiOperationFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var relativePath = context.ApiDescription.RelativePath ?? string.Empty;
		if (!relativePath.StartsWith("agent", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		// 1. Demand API Key authentication for all /agent endpoints
		operation.Security ??= new List<OpenApiSecurityRequirement>();
		operation.Security.Add(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "ApiKey"
					}
				},
				Array.Empty<string>()
			}
		});

		// 2. Put under the "Agent" tag segment
		operation.Tags ??= new List<OpenApiTag>();
		if (!operation.Tags.Any(tag => string.Equals(tag.Name, "Agent", StringComparison.OrdinalIgnoreCase)))
		{
			operation.Tags.Add(new OpenApiTag
			{
				Name = "Agent",
				Description = "Endpoints for external conversational agent platforms."
			});
		}

		// 3. Document standard error responses
		operation.Responses.TryAdd("401", new OpenApiResponse { Description = "X-Api-Key header is missing or invalid." });
		operation.Responses.TryAdd("403", new OpenApiResponse { Description = "The client lacks permissions to invoke this endpoint." });

		// 4. Document purpose and parameters of /api/epicweb/agent/chat
		if (string.Equals(relativePath, "api/epicweb/agent/chat", StringComparison.OrdinalIgnoreCase))
		{
			operation.OperationId = "AgentChat";
			operation.Summary = "Send a conversational query to the AI assistant";
			operation.Description = @"Processes a chat request or conversational segment for the CMS editor experience.

### Capabilities and System Rules:
* **What it does**: Allows external agent platforms, workflows, or conversational clients to directly communicate with and leverage the Optimizely AI Assistant
* **Function Calling**: Supports tool/MCP invocations when useTools is true
* **Safe by Default**: Suggested changes are saved as unpublished drafts; no automatic publishing occurs
* **Permissions**: Respects standard Optimizely CMS access control based on the authenticated user
* **Context Aware**: Attachments and chat history provide conversational continuity";
		}
	}
}
```

#### 2. SchemaFilter Class (Optional)

Add descriptions to request/response schema properties:

```csharp
using System.ComponentModel;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class DescriptionAttributeFilter : ISchemaFilter
{
	private static readonly Dictionary<string, string> StandardPropertyDescriptions = new()
	{
		{ "text", "The user message or query to send to the AI assistant." },
		{ "context", "The current content context (e.g., contentId='152_12') of the currently selected page or block." },
		{ "isHtml", "True if the input 'text' contains HTML tags and needs to maintain or format raw markup context." },
		{ "chatHistory", "The conversation history list containing prior context exchanges for continuation." },
		{ "sessionId", "Unique identifier for the user's current chat session." },
		{ "useTools", "Enables or disables function-calling tool executes. Set to false to force a strictly read-only session." }
	};

	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (schema.Properties == null) return;

		foreach (var property in schema.Properties)
		{
			if (StandardPropertyDescriptions.TryGetValue(property.Key, out var desc))
			{
				property.Value.Description = desc;
			}
		}
	}
}
```

#### 3. Register Filters in ConfigureServices

```csharp
services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
	{
		Name = "X-Api-Key",
		Type = SecuritySchemeType.ApiKey,
		In = ParameterLocation.Header,
		Description = "Send your API key in the X-Api-Key header."
	});

	options.OperationFilter<AgentApiOperationFilter>();
	options.SchemaFilter<DescriptionAttributeFilter>();
});
```

### Option B: .NET 10 Microsoft OpenAPI Support

If your project targets .NET 10 with built-in OpenAPI support (`Microsoft.AspNetCore.OpenApi`):

```csharp
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

services.AddOpenApi(options =>
{
	// Register API key security scheme description
	options.AddDocumentTransformer((document, context, cancellationToken) =>
	{
		document.Components ??= new OpenApiComponents();
		document.Components.SecuritySchemes.Add("ApiKey", new OpenApiSecurityScheme
		{
			Name = "X-Api-Key",
			Type = SecuritySchemeType.ApiKey,
			In = ParameterLocation.Header,
			Description = "Master API Key for external agent platform authentication."
		});
		return Task.CompletedTask;
	});

	// Apply security requirements and operation descriptions for all /agent endpoints
	options.AddOperationTransformer((operation, context, cancellationToken) =>
	{
		var relativePath = context.Description.RelativePath ?? string.Empty;
		if (relativePath.StartsWith("agent", StringComparison.OrdinalIgnoreCase))
		{
			// Attach API key requirements
			operation.Security = new List<OpenApiSecurityRequirement>
			{
				new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "ApiKey"
							}
						},
						Array.Empty<string>()
					}
				}
			};

			// Describe agent/chat endpoint behavior
			if (string.Equals(relativePath, "api/epicweb/agent/chat", StringComparison.OrdinalIgnoreCase))
			{
				operation.Summary = "Send a conversational query to the AI assistant";
				operation.Description = "Connects external agent frameworks to the Optimizely AI Assistant. Read-only by default, but supports suggesting changes saved as unpublished drafts subject to standard CMS approval processes.";
			}
		}
		return Task.CompletedTask;
	});
});
```

---

## Real-World Usage Examples

### Example 1: Basic Text Prompt

```bash
curl -X POST "https://your-cms.com/api/epicweb/agent/chat" \
  -H "X-Api-Key: replace-with-long-random-secret" \
  -H "Content-Type: application/json" \
  -d '{
	"text": "Generate a catchy tagline for our summer sale",
	"sessionId": "session-123"
  }'
```

### Example 2: Context-Aware Prompt with Attachments

```bash
curl -X POST "https://your-cms.com/api/epicweb/agent/chat" \
  -H "X-Api-Key: replace-with-long-random-secret" \
  -H "Content-Type: application/json" \
  -d '{
	"text": "Improve the SEO title for the page with id 152_12",
	"context": {
	  "contentId": "152_12",
	  "contentType": "StandardPage"
	},
	"attachedContent": [
	  {
		"contentId": "42_7",
		"contentType": "ImageData"
	  }
	],
	"sessionId": "session-456"
  }'
```

### Example 3: Function-Calling for Dynamic Content

```bash
curl -X POST "https://your-cms.com/api/epicweb/agent/chat" \
  -H "X-Api-Key: replace-with-long-random-secret" \
  -H "Content-Type: application/json" \
  -d '{
	"text": "Find page and list all properties on 'about us'",
	"useTools": true,
	"sessionId": "session-789"
  }'
```

---

## Integration Scenarios

### Scenario 1: Multi-Agent Orchestration Platform

An external AI orchestration platform (e.g., Autogen, LangChain, LlamaIndex) uses Agent API to offload Optimizely-specific tasks:

1. Platform formulates user query with rich context
2. Calls `/api/epicweb/agent/chat` with `useTools: true`
3. Receives structured response with tool results
4. Integrates response into larger workflow

**Benefits:**
- Delegates Optimizely expertise to native AI Assistant
- Maintains full workflow context in parent platform
- Leverages built-in MCP tools without reinvention

### Scenario 2: Workflow Automation & Batch Processing

A scheduled automation or batch job needs AI-assisted content creation:

1. Job reads batch of content items
2. For each item, sends prompt to Agent API with context
3. Stores suggested changes as drafts
4. Editor reviews and publishes manually

**Benefits:**
- Respects approval workflows (drafts only)
- Reduces manual content creation effort
- No editor involvement until review stage

### Scenario 3: Custom Analytics & Monitoring

Monitor AI Assistant usage across external platforms:

1. Each external agent sends unique `sessionId` and `agentName`
2. Log requests and responses server-side
3. Track usage patterns, token consumption, tool invocations
4. Generate reports on automation effectiveness

**Benefits:**
- Full audit trail of AI-powered changes
- Visibility into external system behavior
- Cost attribution per agent/platform

---

## Session Management

The `ChatSessionCacheService` maintains session state in memory:

- **Cache key:** Combination of `sessionId` and authenticated user
- **TTL:** Configurable (default: 24 hours)
- **Storage:** In-memory only (lost on app restart)

For persistent session storage, implement `IChatSessionCacheService`:

```csharp
public class PersistentSessionCacheService : IChatSessionCacheService
{
	private readonly IRepository<AgentSession> repository;

	public async Task<ChatHistory?> GetAsync(string sessionId, CancellationToken cancellationToken)
	{
		var session = await repository.GetByIdAsync(sessionId, cancellationToken);
		return session?.History;
	}

	public async Task StoreAsync(string sessionId, ChatHistory history, CancellationToken cancellationToken)
	{
		var session = new AgentSession { Id = sessionId, History = history };
		await repository.SaveAsync(session, cancellationToken);
	}
}

// Register in Startup.cs
services.AddScoped<IChatSessionCacheService, PersistentSessionCacheService>();
```

---

## Best Practices

### Security

✅ **DO:**
- Store API keys in secure configuration (environment variables, Azure Key Vault)
- Use long, random API keys (minimum 32 characters recommended)
- Rotate API keys regularly
- Use HTTPS for all Agent API calls
- Implement rate limiting on the endpoint
- Log authentication failures for monitoring

❌ **DON'T:**
- Commit API keys to version control
- Use weak or guessable secrets
- Expose API keys in client-side code
- Send API key in URL parameters

### Performance

✅ **DO:**
- Batch requests when possible to reduce latency
- Enable session caching for conversation continuity
- Monitor token usage and set appropriate limits
- Use read-only mode (`useTools: false`) for non-critical queries

❌ **DON'T:**
- Create hundreds of parallel requests
- Include massive chat histories (prune old messages)
- Fetch unnecessary content attachments
- Run AI operations in synchronous request paths

### Error Handling

✅ **DO:**
- Implement exponential backoff for retries
- Log error responses with correlation IDs
- Provide meaningful error messages to end users
- Validate input payloads before sending

❌ **DON'T:**
- Ignore HTTP error codes
- Send unlimited retry requests
- Expose internal error details to external users

---

## Troubleshooting

### "401 Unauthorized: Missing API key"

**Cause:** `X-Api-Key` header is missing or empty

**Solution:**
```bash
# Verify header is present
curl -i -H "X-Api-Key: your-actual-key" https://your-cms.com/api/epicweb/agent/chat
```

### "401 Unauthorized: Invalid API key"

**Cause:** API key does not match configured keys or validator rejects it

**Solution:**
1. Verify API key matches exactly in `appsettings.json`
2. Check that configured CMS user exists and is active
3. Review authentication handler logs

### "403 Forbidden: Permission denied"

**Cause:** Authenticated CMS user lacks access to requested content

**Solution:**
1. Ensure mapped CMS user has read access to content
2. Check content publishing status
3. Verify user role permissions in Optimizely

### "501 Service Not Implemented: service_not_registered"

**Cause:** `AIAssistantService` not registered in dependency injection

**Solution:**
```csharp
// In Startup.cs, ensure this is present:
services.AddAIAssistant();  // Before AddEpicwebAgentApi()
services.AddEpicwebAgentApi();
```

### Session not persisting across requests

**Cause:** Default in-memory cache lost on app restart

**Solution:**
Implement persistent `IChatSessionCacheService` (see **Session Management** section)

---

## API Reference

### Request Headers

| Header | Required | Value |
|--------|----------|-------|
| `Content-Type` | ✓ | `application/json` |
| `X-Api-Key` | ✓ (if using built-in auth) | Your configured API key |
| `Authorization` | ✓ (if using JWT) | `Bearer <token>` |

### Response Headers

| Header | Value |
|--------|-------|
| `Content-Type` | `application/json` |
| `X-Request-Id` | Correlation ID for debugging |

### HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success; AI response returned |
| 400 | Bad request (invalid JSON, missing fields) |
| 401 | Unauthorized (missing or invalid credentials) |
| 403 | Forbidden (user lacks content access) |
| 501 | Service not available |

---

## Frequently Asked Questions (FAQ)

### What is the Agent API?

The Agent API is a standardized REST endpoint (`/api/epicweb/agent/chat`) that allows external assistant engines, automation platforms, and workflows to interact directly with the Optimizely AI Assistant. It enables seamless AI-powered integration from third-party systems while leveraging all built-in AI capabilities, tools, and content management features.

**Prerequisites:** You must have the base **Epicweb.Optimizely.AIAssistant** package installed with the **Chat** module enabled. The Agent API extends the existing AI Assistant chat functionality to programmatic access.

### How is Agent API different from the editor UI?

- **Editor UI**: Interactive chat for CMS editors within the Optimizely admin interface
- **Agent API**: Programmatic REST endpoint for external systems, bots, and automation platforms to invoke AI assistance without user interaction

Both use the same underlying AI service and respect the same content permissions.

### Can I use Agent API for automation and batch processing?

Yes. Common use cases include:
- Batch content generation and suggestions
- AI-powered workflow automation
- Integration with external orchestration platforms (e.g., Autogen, LangChain)
- Scheduled jobs that need AI-assisted content creation
- Multi-agent systems coordinating CMS updates

Suggested changes are saved as **unpublished drafts**, so an editor must review and approve before publishing—no automatic publishing occurs.

### Who has access to the Agent API?

Each Agent API key is mapped to an **Optimizely CMS user account**. The agent inherits all the permissions, roles, and access restrictions of that user. Use your standard CMS admin UI to manage agent user roles and content access—no special API permissions needed.

For example, if you map an agent to a user with `editor` role, the agent can only access and modify content that user can edit.

### How do I authenticate Agent API requests?

By default, the Agent API uses **API key authentication** (`X-Api-Key` header). Configure your keys in `appsettings.json`:

```json
{
  "Epicweb": {
    "AIAssistant": {
      "AgentApi": {
        "Agents": {
          "MyAgent": {
            "ApiKey": "your-long-secret-key",
            "CmsUserName": "editor"
          }
        }
      }
    }
  }
}
```

Then send requests with the header:
```http
X-Api-Key: your-long-secret-key
```

You can also implement **JWT bearer tokens** or custom authentication by swapping the authentication scheme.

### Can Agent API invoke tools (MCP)?

Yes. Set `"useTools": true` in your request payload to enable the agent to invoke available MCP tools (e.g., search content, create pages, update properties). This respects the same function-calling permissions as the editor UI.

Set `"useTools": false` for read-only sessions where the AI cannot perform actions.

### Is Agent API suitable for high-volume requests?

The Agent API is designed for moderate to high throughput. For optimal performance:
- Batch requests when possible
- Implement exponential backoff for retries
- Monitor token usage and set appropriate rate limits
- Use read-only mode (`useTools: false`) for non-critical queries
- Enable session caching for conversation continuity

For very high-volume scenarios (thousands of requests/day), contact us to discuss enterprise options.

### Does Agent API support session persistence?

Yes. Each request can include a `sessionId` to maintain conversation history across multiple calls. By default, sessions are cached in memory and expire after 24 hours.

For persistent session storage (e.g., database), implement the `IChatSessionCacheService` interface and register it in dependency injection.

### Can I use Agent API with custom AI models?

Yes. The Agent API uses your configured AI provider:
- OpenAI (default)
- Azure OpenAI
- Google Gemini
- Custom AI provider with your own models

Configure your AI provider in `appsettings.json` and all Agent API requests automatically use it.

### What happens if the Agent API service is unavailable?

The endpoint returns a `501 Service Not Implemented` error if the underlying `AIAssistantService` is not registered. Ensure:
1. `services.AddAIAssistant()` is called before `services.AddEpicwebAgentApi()`
2. Your AI provider credentials are valid
3. Your CMS host application is running

### Can I use Agent API with content that has publishing restrictions?

Yes. Agent API respects all Optimizely content restrictions:
- Access control lists (ACLs)
- Publishing schedules
- Workflow states
- User role permissions

The mapped CMS user must have the appropriate permissions to access and suggest changes to content.

### Does Agent API support context and attachments?

Yes. You can provide:
- **Context**: Current page/block being edited (e.g., in the message or `"context": { "contentId": "152_12" }`)
- **Attachments**: Additional content items as reference (e.g., related pages, images)
- **Chat history**: Prior conversation for continuity, this part is cached in memory for 24 hours by default and held by sessionId.

All context is used to improve AI relevance and accuracy.

### How are suggested changes handled?

Suggested changes via Agent API are saved as **unpublished drafts** by default. They:
- Do not affect published content
- Are visible in the CMS UI for editor review
- Require manual approval before publishing
- Respect standard CMS approval workflows

This ensures AI suggestions are always reviewed by a human before going live.

---

## See Also

- [Configuration Guide](configuration.md) - Full AI Assistant configuration options
- [Tools & MCP Integration](tools.md) - Available built-in and custom tools
- [Installation Guide](installation.md) - Complete setup instructions
- [FAQ](faq.md) - Common questions and answers
