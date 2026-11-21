# The AI Assistant for Optimizely Content Cloud (CMS) PaaS and Commerce Connect PaaS

## Epicweb.Optimizely.AIAssistant Plugin
The plugin offers intuitive support within the Optimizely CMS and Customizable Commerce platform, providing suggestions, translations, SEO summarization, formatting help, AI chat and much more for input fields within the Optimizely CMS and Commerce Connect platform

The packages support Optimizely CMS 12 and CMS 13. See [Installation](installation.md) for package and runtime requirements.

## Links 
- [Installation](installation.md)
- [Configuration](configuration.md)
- [License](license.md) (Free for evaluation, premium plan for Production use)
- Payment plans ([Read FAQ](faq.md))
- [User Manual](user-manual.md)
- [SEO Manager](seo-manager.md)
- [Change log](change-log.md)
- **[AI Chat - Getting Started](chat-instructions.md)** ⭐ NEW
- **[AI Tools - Function Calling and MCP](tools.md)** ⭐ NEW
- **[Agent API - External Integration](agent-api.md)** ⭐ NEW
- **[Administration Features](admin-features.md)** ⭐ NEW - Instructions, Assistants & Statistics
- **[Shortcuts Guide](promptshortcuts.md)** - Speed up common tasks with reusable prompts
- [FAQ](faq.md)
- [Feedback and Issues](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)
- [Example Commerce Connect](example-Commerce-work-with-SEO-and-translations-on-a-product.md)

### GET STARTED

For Premium Subscription: https://aiassistant.optimizely.blog

For a free evaluation without any licensing messages in the production environment, please complete the form at https://aiassistant.optimizely.blog

**Calculator** - Estimate how much time and money your organization could save by using AI Assistant in Optimizely CMS.: https://aiassistant.optimizely.blog/en/calculator/

## Feature list
- Provides suggestions or alternatives for your text
- Translates your text into other languages
- Image generation
- Image transformation
- Image analyzing
- Image Alttext automation
- SEO title generations
- SEO description generation
- SEO Manager for finding and reviewing SEO metadata issues across CMS pages
- Generates new text
- Generates FAQ questions and answers (AEO)
- Tools for AI – Connect your C# Function Calling or MCP
- RAG (Retrieval-Augmented Generation) – Connect your own knowledge base
- **AI Chat Window** - Conversational AI assistant for content editors ⭐ NEW
- **Agent API** - REST API for external AI integrations and headless content operations ⭐ NEW
- **Custom instructions** - Possibility to add your own shortcut instructions from the UI ⭐ NEW
- **Custom Assistants** - Possibility to add your own AI experts from the UI ⭐ NEW
- Spell-checking
- Change or maintain consistency in tone of writing
- Extracts keywords from your text
- Run your own prompts to ChatGPT/Gemini
- Summarize texts and other property fields on page/site (SEO, GEO)
- HTML formatting and accessibility guidance
- **[Add your own shortcut prompts](promptshortcuts.md)** - Create reusable AI actions on field level
- Use ChatGPT/Gemini as inline help
- Use any AI of your choice (Bring your own AI)

The AI assistant is a powerful and intuitive tool that will help you in your everyday work!

## Agent API - External Integration

The **Agent API** enables external applications to leverage AI capabilities for headless content operations and integrations:

- **REST API endpoint** (`/api/epicweb/agent/chat`) for programmatic access
- **API key authentication** with CMS user mapping, or a custom ASP.NET Core authentication scheme
- **Access to AI tools** for content creation, updates, publishing, and more
- **Multi-language support** for content operations across site languages
- **Visitor groups and personalization** through available tools
- **Perfect for** headless CMS use cases, third-party integrations, custom agent workflows

👉 **See [Agent API Documentation](agent-api.md) for setup, authentication, and usage examples**

## Built-in providers:

- OpenAI
- Google Gemini
- [Azure OpenAI](configuration-azure-openai.md)
- [Custom AI](custom-ai.md) for a compatible REST integration with your preferred model or service
- Image generation can use a separate provider. See [Image generation](example-image-generation.md) and the [Recraft setup](recraftai-image-generation.md).

## Video shorts and guides

[Go to our video portal for guides](https://aiassistant.optimizely.blog/en/videos/)

## Dependencies

| Package series | Target framework | Optimizely dependency |
| --- | --- | --- |
| 4.x for CMS 12 | .NET 8 | `EPiServer.CMS` 12.18.0 or later within 12.x |
| 5.x for CMS 13 | .NET 10 | `EPiServer.CMS` and `EPiServer.Logging` 13.x |

## Discussions and feedback

This can be discussed in Github discussions (https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)

## Documentation maintainer

https://github.com/lucgosso

## TERMS OF USE

[License](license.md)
