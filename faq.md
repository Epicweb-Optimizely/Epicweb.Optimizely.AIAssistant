# FAQ

## What is the AI-assistant for Optimizely?
The AI assistant is created to facilitate and simplify the workflow for you as an editor. It is fully integrated into all fields in the platform/Optimizely, and it will help you with your writing and streamline many of your time-consuming editorial activities in a smooth and easy way.

## How does it work? 
The AI assistant can be accessed directly via the editor mode and offers automated suggestions for your text content and alternative phrasings, while also providing spelling checks and translation of texts and properties on pages and blocks. It has smart pre-installed commands for text summarization and can change the tone (persuasive, informative, etc.) of the text. It can also generate FAQs, tables, summaries, images, and image captions with just a few clicks.

- Can be accessed directly via the editor mode
- Offers and suggests automated suggestions, content, and phrasings for your text
- Generates smooth translations into different languages
- Produces exceptional realistic images or vector graphics
- Image Analyzer functionality for accurate Alt Texts and meta extraction
- Offers spelling checks
- Offers the ability to change and fine-tune the tone of a text (persuasive, informative, etc.)
- Can summarize and condense texts
- Generates FAQs, tables, images, and image captions with just a few clicks
- Formatting tables and lists, WCAG compatible
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

We offer flexible 6-month or 12-month payment plans.

To ensure your satisfaction, we also provide a three-month money-back guarantee — we're committed to excellence, every step of the way.

## Which AI LLMs can i use? 

With our Custom AI provider, you can connect any AI to the service [Read about the CustomAI-provider](custom-ai.md)

- GPT-4 and 5, OpenAI
- Claude, Anthropic
- Gemini, Google DeepMind
- Jurassic-2, AI21 Labs
- LLaMA, Meta
- NeMo, NVIDIA
- Command R, Cohere
- Tongyi Qianwen, Alibaba Cloud
- Ernie, Baidu
- StableLM, Stability AI
- Grok, xAI
- Einstein GPT, Salesforce
- ChatGLM, Tsinghua University
- Grok, KakaoBrain
- Mistral, Mistral AI
- writer.com
- Bring your own AI

## Will the data be used as part of the learning data for AI? 
No, OpenAI will not use data submitted by customers via our API to train or improve our models. Source: https://openai.com/policies/api-data-usage-policies

Google does not use your prompts or outputs to improve Google products. Source: https://ai.google.dev/gemini-api/terms 

## Do you offer EU data residency?

Yes, we are now offering integration to Azure OpenAI Service which are offering AI APIs from different parts of Europe (Sweden Central, France Central,  UK South) and enables localized data storage. https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models

Data privacy and compliance are crucial in the current GDPR and data protection landscape. We enable clients to store their data exclusively within the EU, ensuring compliance and building trust. This is especially beneficial for businesses in Sweden and the UK, as it provides an additional level of confidence with localized data centers.

## Do you offer AI assistance on your own data?

YES! we can offer to setup your own models in Azure and finetune them.

Thru our Custom AI provider we allow you to incorporate custom logic and utilize your preferred Large Language Models (LLMs) such as Meta Llama AI, Google Gemini AI, Anthropic Claude AI, or any other AI that offers a REST API. [Documented here](custom-ai.md)

Through custom tools, we offer the retrieval of individual data and real-time data from external systems, referred to as RAG. [Documented here](configuration-ai-tools.md)

## Can I use/sell the images I create with the AI-Assistant Image Generation Tool?

Subject to the Content Policy and Terms of OpenAI DALL·E, you own the images you create with the AI-Assistant (DALL·E), including the right to reprint, sell, and merchandise. 
Source:  https://help.openai.com/en/articles/6425277-can-i-sell-images-i-create-with-dall-e 

Same goes with Recraft AI: When you are using the paid API subscription, you own the assets you create. Recraft assigns you the copyright rights to those assets, allowing you to use them for both personal and commercial purposes. However, you are prohibited from using these assets to train AI models, systems, or similar technologies.
Source: https://www.recraft.ai/terms

Under Google’s Gemini API Additional Terms of Service, Google does not claim ownership over the content (including images) you generate using the service.  Source: https://ai.google.dev/gemini-api/terms 

## How do translations work?
OpenAI's service supports over 92 languages, offering some of the most accurate and high-quality translations available worldwide. Additionally, you can provide specific terms or translation guidelines to tailor the translation process to your needs.

Google’s Translation LLM (powered by Gemini) on Vertex AI lists 49 supported source/target languages as of 7 Oct 2025. See the “[Supported languages](https://cloud.google.com/vertex-ai/generative-ai/docs/translate/translate-text#supported-languages)” table on Google’s documentation.

## Is the AI-Assistant Plugin EU AI ACT compatible?
Yes, the AI-Assistant Plugin for Optimizely is designed to be compatible with the EU AI Act. Here’s a summary of why it aligns with the Act’s requirements:

- Transparency: Clearly informs users that content is AI-generated, keeping all outputs in draft mode for human review.
- Human Oversight: Allows editors to review, accept, or reject AI-generated content, ensuring human control over the final result.
- Data Quality Responsibility: The AI-Assistant allows customers to choose their preferred models for content generation. Ensuring the use of diverse and fair datasets is handled by the implementing partner or customer.
- Limited Risk Category: The application is used in editorial support, a lower-risk use case, not falling under high-risk or prohibited categories.
- Compliance with GDPR: Ensures data privacy and security, aligning with broader EU data protection laws.
- Non-manipulative Use: The tool enhances editorial efficiency without manipulative or harmful purposes.

- This ensures the AI-Assistant Plugin meets the transparency, safety, and fairness standards set by the EU AI Act.

---

## Agent API - External Integration

### What is the Agent API?

The Agent API is a standardized REST endpoint (`/api/epicweb/agent/chat`) that allows external assistant engines, automation platforms, and workflows to interact directly with the Optimizely AI Assistant. It enables seamless AI-powered integration from third-party systems while leveraging all built-in AI capabilities, tools, and content management features.

**Prerequisites:** You must have the base **Epicweb.Optimizely.AIAssistant** package installed with the **Chat** module enabled. The Agent API extends the existing AI Assistant chat functionality to programmatic access.

[Read full Agent API documentation](agent-api.md)

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


