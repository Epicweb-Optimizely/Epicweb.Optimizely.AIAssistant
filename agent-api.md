# Agent API - External Integration Endpoint

## Overview

The Agent API lets external applications send messages to the Optimizely AI Assistant at `POST /api/epicweb/agent/chat`. A request can identify CMS content, continue a conversation, and let the assistant call registered chat tools.

## Security Model: CMS Identity & Permissions

The chat endpoint authenticates the caller and resolves a CMS user on the server. With API key authentication, each configured credential maps to a CMS user name. Tools that access or modify content use the mapped CMS identity. Give each integration a CMS user with the permissions it needs for its workflow.

Create and update tools can save drafts. The Publish tool pack includes actions that can publish, unpublish, or schedule content when the mapped user has publish access. For a draft-only workflow, give the account permission to edit without granting publish access.

## Architecture & Core Concepts

The controller authenticates chat requests and resolves the CMS principal before invoking `AIAssistantService`. The service sends the conversation to the configured provider and processes calls to available chat tools. The controller returns the result and stores updated chat history when the service provides it.

## Setup and Installation

### 1. Add Project Reference

Add a reference to the `Epicweb.Optimizely.AIAssistant.AgentAPI` package in your Optimizely CMS project.

### 2. Register in Startup.cs or Program.cs

```csharp
using Epicweb.Optimizely.AIAssistant;
using Epicweb.Optimizely.AIAssistant.AgentAPI;

// In ConfigureServices or on builder.Services:
services.AddAIAssistant();
services.AddEpicwebAgentApi();
```

`AddAIAssistantAgentAPI()` is an alias for the default registration. Ensure the host maps controller endpoints (for example, with `app.MapControllers()` in a minimal hosting application).

### 3. Configure Route (Optional)

The default route prefix is `/api/epicweb/agent`. Set `Epicweb:AIAssistant:AgentApi:Path` in configuration or pass `agentApiPath` to the registration method.

```csharp
services.AddEpicwebAgentApi(agentApiPath: "/api/agents");
```

With this prefix, send chat requests to `/api/agents/chat` and find the discovery endpoints at `/api/agents/health` and `/api/agents/openapi.json`.

## Security & Authentication

### Default API Key Configuration

The default registration protects chat with policy `Epicweb.AgentApi` and the `X-Api-Key` header. Configure credentials and their CMS identities under `Epicweb:AIAssistant:AgentApi`. Use secure configuration for the actual keys.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "AgentApi": {
        "DefaultAgent": {
          "ApiKey": "<default-agent-secret>",
          "AgentName": "EpicwebAgent",
          "CmsUserName": "integration-editor"
        },
        "Agents": {
          "MyAgent": {
            "ApiKey": "<my-agent-secret>",
            "CmsUserName": "integration-reader"
          }
        }
      }
    }
  }
}
```

Each entry needs an `ApiKey` and `CmsUserName`. For `DefaultAgent`, the optional `AgentName` defaults to `EpicwebAgent`. Named entries use the configuration entry name as the agent name.

The built-in validator also checks one legacy credential after the Agent API entries. Under `Epicweb:AIAssistant`, it uses `ApiKeys:Agents` if set, otherwise the only `ApiKeys` entry if there is exactly one, and then `ApiKey` as a fallback. The legacy credential maps to `AgentApi:DefaultAgent:CmsUserName` or `AgentApi:DefaultCmsUserName`, which defaults to `AI-Agent`.

### JWT Bearer Configuration (Custom Authentication)

Register the host's bearer scheme and configure the Agent API policy to use it.

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;

services.AddAuthentication()
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

Send the token as `Authorization: Bearer <token>`. The default CMS user resolver reads the `epicweb:cms-user` claim. Provide the claim through your trusted issuer or register an `IAgentCmsUserResolver` that maps the authenticated principal to a CMS user.

### Custom Authentication Implementation

With built-in API key authentication, register your own `IAgentApiKeyValidator` before calling `AddEpicwebAgentApi()`. Its `ValidateAsync` method returns an `AgentApiAuthenticationResult` with `Succeeded`, `AgentName`, and `CmsUserName` for successful validation. The authentication handler uses the result to populate the authenticated principal.

```csharp
using Epicweb.Optimizely.AIAssistant.AgentAPI.Authentication;

// CustomApiKeyValidator implements IAgentApiKeyValidator.
services.AddScoped<IAgentApiKeyValidator, CustomApiKeyValidator>();
services.AddEpicwebAgentApi();
```

For a custom authentication scheme, register an `IAgentApiOpenApiSecurityProvider` to describe that scheme in the discovery document. The default provider documents `X-Api-Key`.

## Discovery Endpoints

Both discovery endpoints allow anonymous access.

### Health Check

`GET /api/epicweb/agent/health` returns the service name, UTC timestamp, status, and configured endpoint paths.

### OpenAPI Document

`GET /api/epicweb/agent/openapi.json` returns an OpenAPI 3.1.1 document with schemas, examples, and the configured security definition for chat.

## Chat Request/Response

### POST /api/epicweb/agent/chat

```json
{
  "text": "Summarize the current page and suggest a shorter teaser",
  "context": {
    "id": "152_12",
    "language": "en"
  },
  "sessionId": "0b507f7e-b063-428f-a304-8f01b51c021e",
  "useTools": true
}
```

| Field | Description |
|-------|-------------|
| `text` | Message for the assistant |
| `context` | Current CMS context. Use `context.id` and `context.language` or supply `contentid` and `lang` at the top level. |
| `sessionId` | Client-generated conversation ID to reuse for follow-up requests |
| `chatHistory` | Prior messages to seed a conversation when the server has no cached history |
| `instructions` | Additional instructions for the default Agent chat action |
| `agentName` | Caller-supplied metadata. The authenticated agent name takes precedence when available. |
| `useTools` | Set to `true` to request chat tools or `false` to omit tool definitions from an ordinary request. Omitting the field allows tools as needed. |
| `isHtml` | Indicates that the input contains HTML |

To reference additional pages, blocks, or media, identify them in the message so available tools can retrieve them using the mapped CMS user's permissions.

### Successful Response (200 OK)

The endpoint returns `AiChatResponse`. This example shows selected fields.

```json
{
  "text": "The page introduces the summer collection and links to featured products.",
  "sessionId": "0b507f7e-b063-428f-a304-8f01b51c021e",
  "correlationId": "252248b3-93be-47e6-b257-e784e11a3f65",
  "tokenUsage": {
    "currentInputTokens": 120,
    "currentOutputTokens": 85,
    "currentTotalTokens": 205
  },
  "updatedChatHistory": [
    {
      "role": "user",
      "content": "Summarize the current page and suggest a shorter teaser"
    },
    {
      "role": "assistant",
      "content": "The page introduces the summer collection and links to featured products."
    }
  ]
}
```

When updated history is available, the controller omits system messages from the returned history and summarizes them as `toolCalls` entries with a `status` field.

### Error Responses

| Status | Meaning |
|--------|---------|
| 400 | Missing or invalid request body. MVC may return a validation problem response. |
| 401 | Missing or invalid credentials |
| 403 | The authenticated agent could not be mapped to a valid CMS identity (`cms_identity_missing` or `cms_identity_invalid`) |
| 500 | The CMS principal service or accessor is unavailable, or principal initialization failed |
| 501 | `AIAssistantService` is missing from dependency injection (`service_not_registered`) |

A tool can report a content access or operation failure in the chat response.

## OpenAPI & Swagger Documentation

Import the [built-in discovery document](#openapi-document) into your API client. To include the chat action in a host-generated Swagger or OpenAPI document, apply the host's security configuration to `AgentController.Chat`.

## Real-World Usage Examples

### Example 1: Basic Text Prompt

```bash
curl -X POST "https://your-cms.com/api/epicweb/agent/chat" \
  -H "X-Api-Key: <agent-api-key>" \
  -H "Content-Type: application/json" \
  -d '{"text":"Generate a tagline for the summer sale","useTools":false}'
```

### Example 2: Context-Aware Prompt

```bash
curl -X POST "https://your-cms.com/api/epicweb/agent/chat" \
  -H "X-Api-Key: <agent-api-key>" \
  -H "Content-Type: application/json" \
  -d '{
    "text": "Read this page and suggest a shorter SEO title",
    "context": { "id": "152_12", "language": "en" },
    "sessionId": "0b507f7e-b063-428f-a304-8f01b51c021e",
    "useTools": true
  }'
```

### Example 3: Function-Calling for Dynamic Content

```bash
curl -X POST "https://your-cms.com/api/epicweb/agent/chat" \
  -H "X-Api-Key: <agent-api-key>" \
  -H "Content-Type: application/json" \
  -d '{"text":"Find the About Us page and list its properties","useTools":true}'
```

## Integration Scenarios

- Send a page summary request from an external workflow platform and include the result in its output.
- Request SEO suggestions for individual pages in a scheduled job and deliver the returned text for editorial review.
- Record `correlationId` and `tokenUsage` from responses for troubleshooting and usage tracking.

## Session Management

Generate a unique, random `sessionId` per conversation and reuse it for each follow-up. The default cache looks up history by session ID alone. Keep each ID within its intended conversation. When a cache entry exists, the server uses its history and tool definitions. Supply `chatHistory` to seed a conversation when there is no cache entry.

The default implementation stores history in the current application instance for three hours after the most recent save. Restarting that instance clears its cache. For a shared or persistent cache, implement `IChatSessionCacheService` with `SaveChatHistory`, `GetChatHistory`, and `RemoveChatHistory`. Register your implementation in dependency injection before calling `AddEpicwebAgentApi()`.

## Best Practices

- Store API keys in secure configuration and send them over HTTPS.
- Use a dedicated Optimizely CMS account for each integration and grant the permissions required for its content operations.
- Use an account with read access for a read-only content integration. Set `useTools: false` for text requests that need no tool input.
- Reuse a conversation's session ID when continuing it, and monitor token usage for bulk processing.

## Troubleshooting

- **401 Unauthorized**. Verify the request has the required authentication header and the credential matches the configured scheme.
- **403 Forbidden**. Verify that the mapped CMS user exists and that the CMS user resolver can resolve the authenticated principal.
- **501 Not Implemented**. Verify that the host calls `AddAIAssistant()`.
- **Missing conversation history**. Verify that each turn uses the same `sessionId`. If requests reach different application instances, use a shared cache.

## API Reference

### Request Headers

| Header | Use |
|--------|-----|
| `Content-Type: application/json` | All chat requests |
| `X-Api-Key: <agent-api-key>` | Built-in API key authentication |
| `Authorization: Bearer <token>` | When the host configures bearer authentication |

See [Chat Request/Response](#chat-requestresponse) for the JSON fields and HTTP status codes.

## Frequently Asked Questions (FAQ)

### Can I use Agent API with custom AI models?

The Agent API uses the AI Assistant provider configured for the CMS host. See the [Configuration Guide](configuration.md).

### How are suggested changes handled?

A text suggestion appears in the chat response. A tool can change content when the mapped user has the required CMS permissions. The available tools and CMS access determine whether the integration can save drafts, publish, or schedule content.

## See Also

- [Configuration Guide](configuration.md)
- [Tools & MCP Integration](tools.md)
- [Installation Guide](installation.md)
- [FAQ](faq.md)
