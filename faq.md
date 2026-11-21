# FAQ

## What is the AI-assistant for Optimizely?
The AI assistant is created to facilitate and simplify the workflow for you as an editor. It integrates with enabled Optimizely property editors and offers optional tools such as AI Chat and SEO Manager.

## How does it work? 
The AI assistant can be accessed directly via the editor mode and offers automated suggestions for your text content and alternative phrasings, while also providing spelling checks and translation of texts and properties on pages and blocks. It has smart pre-installed commands for text summarization and can change the tone (persuasive, informative, etc.) of the text. It can also generate FAQs, tables, summaries, images, and image captions with just a few clicks.

- Can be accessed directly via the editor mode
- Offers and suggests automated suggestions, content, and phrasings for your text
- Generates smooth translations into different languages
- Generates images in formats supported by the configured image provider
- Image Analyzer functionality for accurate Alt Texts and meta extraction
- Offers spelling checks
- Offers the ability to change and fine-tune the tone of a text (persuasive, informative, etc.)
- Can summarize and condense texts
- Generates FAQs, tables, images, and image captions with just a few clicks
- Helps format tables and lists and suggest accessibility improvements
- Integrate C# tools with AI Assistant for custom use.
- Use your data for AI-generated responses (RAG).

The AI assistant is a powerful and intuitive tool that will help you in your everyday work!

## Is it free?
It is free for evaluation purposes, but you may encounter limitations and receive license messages. We offer different kinds of packages depending on your needs, contact us for more information about prices. Premium subsciption is our most valuable package.

## What is included in Premium subscription? 
You get 
- Priority Usage
   - Latest and new models will be available for you
- Increased Usage Limits
   - Latest and greatest models and models that can handle more text
- Support
   - Email support, answer within 24h
   - New versions
- Optional Setup with different AI-Models
   - Setup your own models in Azure and finetune them
   - Setup your own custom AI

## How do i get Premium subscription?
Please fill in this form on this page https://aiassistant.optimizely.blog or email support@epicweb.se to get started

## What does Premium subscription cost?
Pricing begins at:

- Starting $400/month for US companies
- Starting 400€/month for European companies
- Starting 3900 SEK/month for companies in Sweden
Please note: Traffic costs are not included in the base price. You pay your own AI-tokens from you AI vendor of choice. 

The price list is modular. Additional costs apply per domain and for optional modules such as the Image Module (generation), altText modules (image analysis), and the Language Manager (automated translation).

We offer 6-month or 12-month payment plans. See [Epicweb's published pricing](https://aiassistant.optimizely.blog/).

To ensure your satisfaction, we also provide a three-month money-back guarantee — we're committed to excellence, every step of the way.

## Which AI LLMs can i use?

Choose a model from a configured provider such as OpenAI, Azure OpenAI, or Google Gemini. To use another model or service, implement the REST contract described in the [Custom AI guide](custom-ai.md).

## Will the data be used as part of the learning data for AI?

Data use depends on the configured provider and account. OpenAI does not train on API business data by default. Google's Gemini API terms distinguish between paid and unpaid services and specify which terms apply to accounts in the EEA, Switzerland, and the United Kingdom. [OpenAI enterprise privacy](https://openai.com/enterprise-privacy/) · [Gemini API terms](https://aistudio.google.com/docs/terms)

## Do you offer EU data residency?

The plugin can send requests to the Azure OpenAI deployment you configure. Azure's processing location depends on the deployment type. EU Data Zone deployments process inference data within that zone, while Global deployments may process requests outside it. Check the deployment and data-handling terms for each service in your setup. [Azure OpenAI data privacy](https://learn.microsoft.com/en-us/legal/cognitive-services/openai/data-privacy)

## Do you offer AI assistance on your own data?

YES! we can offer to setup your own models in Azure and finetune them.

Thru our Custom AI provider we allow you to incorporate custom logic and utilize your preferred Large Language Models (LLMs) such as Meta Llama AI, Google Gemini AI, Anthropic Claude AI, or any other AI that offers a REST API. [Documented here](custom-ai.md)

Through custom tools, we offer the retrieval of individual data and real-time data from external systems, referred to as RAG. [Documented here](tools.md)

## Can I use/sell the images I create with the AI-Assistant Image Generation Tool?

Commercial use depends on the terms for the configured image provider and any third-party rights that apply. See the [OpenAI Services Agreement](https://openai.com/policies/services-agreement/), the [Recraft Developer Terms](https://www.recraft.ai/legal/developer-terms), or the [Gemini API terms](https://aistudio.google.com/docs/terms#use-generated).

## How do translations work?

The plugin sends the text, source and target languages, and any configured translation instructions to your configured AI provider. The available languages depend on the model. See [Translation configuration](configuration-translations.md).

## How does the plugin support editorial review under the EU AI Act?

Editors can review AI suggestions before applying them. SEO Manager offers **Save draft** and a separate **Publish** action for eligible language versions. Chat and Agent API can invoke publishing tools when those tools are registered and the CMS user has permission to publish.

Applicable AI Act requirements depend on the deployment and how it is used. See the [European Commission's guidance on AI transparency obligations](https://digital-strategy.ec.europa.eu/en/policies/guidelines-ai-transparency-obligations).

---

## Agent API - External Integration

### What is the Agent API?

The Agent API is a standardized REST endpoint (`/api/epicweb/agent/chat`) that allows external assistant engines, automation platforms, and workflows to interact directly with the Optimizely AI Assistant. It uses the host's configured AI provider and tools registered for chat.

**Prerequisites:** Install and register the base `Epicweb.Optimizely.AIAssistant` plugin and the separate `Epicweb.Optimizely.AIAssistant.AgentAPI` package. The API calls the same chat service used by the editor UI.

[Read full Agent API documentation](agent-api.md)

### How is Agent API different from the editor UI?

- **Editor UI**: Interactive chat for CMS editors within the Optimizely admin interface
- **Agent API**: Programmatic REST endpoint for external systems, bots, and automation platforms to invoke AI assistance without user interaction

Both use the same underlying AI service. The API runs content operations as the mapped CMS user.

### Can I use Agent API for automation and batch processing?

Yes. Common use cases include:
- Batch content generation and suggestions
- AI-powered workflow automation
- Integration with external orchestration platforms (e.g., Autogen, LangChain)
- Scheduled jobs that need AI-assisted content creation
- Multi-agent systems coordinating CMS updates

Drafting and publishing are separate tool operations. When publishing tools are available and the mapped CMS user has Publish permission, a request can publish or schedule content.

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

### Can Agent API invoke registered tools?

Yes. Set `"useTools": true` to let the assistant invoke registered chat tools such as content search, page creation, or property updates. Set `"useTools": false` to omit tools in an ordinary chat request. Assign the mapped CMS user only the permissions the integration needs.

### Is Agent API suitable for high-volume requests?

Throughput depends on the CMS host, the configured AI service, and the work tools perform. Monitor token usage and account for provider rate limits. For requests that only need AI-generated text, omit tools with `useTools: false`.

For very high-volume scenarios (thousands of requests/day), contact us to discuss enterprise options.

### Does Agent API support session persistence?

Yes. Each request can include a `sessionId` to maintain conversation history across multiple calls. The default in-memory cache expires three hours after the last saved response for that session.

For persistent session storage (e.g., database), implement the `IChatSessionCacheService` interface and register it in dependency injection.

### Can I use Agent API with custom AI models?

Yes. The Agent API uses the host's AI provider configuration. See [Configuration](configuration.md) for provider and model settings.

### What happens if the Agent API service is unavailable?

The chat endpoint returns `501 Not Implemented` with `service_not_registered` when the host has not registered `AIAssistantService`. Register the base plugin with `AddAIAssistant()` and the API extension with `AddAIAssistantAgentAPI()` or `AddEpicwebAgentApi(...)`.

### Can I use Agent API with content that has publishing restrictions?

The API maps requests to a CMS user. Content tools apply the CMS access checks for their operation. For example, publishing requires CMS Publish permission. Review [Agent API setup and permissions](agent-api.md) when configuring an integration.

### How do I pass CMS context to the Agent API?

To identify the current CMS item, set `contentid` and `lang` or pass `"context": { "id": "152_12", "language": "en" }`. Use registered content tools to retrieve other CMS items. The request also supports `chatHistory` for importing history and `sessionId` for continuing a cached conversation. See the [Agent API request reference](agent-api.md).

