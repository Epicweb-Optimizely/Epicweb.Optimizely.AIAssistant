# Installation Epicweb.Optimizely.AIAssistant

The installation should be done by a developer who has access to the source code of your solution.

Install it thru nuget package management:  https://nuget.optimizely.com/package/?id=Epicweb.Optimizely.AIAssistant

For demo and evaluation, the only required configuration is to include the **AI**HINT attribute for the property.

1. Add to startup.cs: services.AddAIAssistant();

2. Decorate your string property with [UIHint(AIHint.Image)], [UIHint(AIHint.Textarea)] or [UIHint(AIHint.Input)] for single line strings

3. Decorate properties with [AIUseToAnalyzeContent] - used to analyze a page or block for important content if referred - add this only on properties that give meaning for the context

4. Add "ai_assistant_execute ai_assistant_choices ai_assistant_image" to Toolbar in TinyMceConfiguration

5. Use [AIAssistant]-attribute to configure properties and give assistant instruction => https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/blob/master/configuration.md#aiassistant

6. For image generations to work, you need to register an account and create a api-key https://platform.openai.com/account/api-keys => then add it to appsettings.json "Epicweb": { "AIAssistant": { "ApiKey": "sk-4ks...." }}

7. For global instructions to AI Assistant => implement IAssistantInstructionsResolver => https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/blob/master/configuration.md#iassistantinstructionsresolver


For Premium Subscription: https://aiassistant.optimizely.blog

For a free evaluation without any licensing messages in the production environment, please complete the form at https://aiassistant.optimizely.blog

- Provides suggestions or alternatives for your text
- Translates your text into other 86+ languages
- Image Generation
- Generates new text
- Spell-checking
- Change tone of writing
- Extracts keywords from your text
- Run your own prompts to ChatGPT
- Summarize texts and other propertyfields on page/site
- Add your own shortcut prompts
- Use ChatGPT as inline help
- Use any AI of your choice

Choose provider:
- Use OpenAI in USA
- Use Azure OpenAI Services in data centers in Europe (Sweden, UK, France) => https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/blob/master/configuration.md#azure-open-ai-provider
- Use our CustomAI-provider. It allows you to incorporate custom logic and utilize your preferred Large Language Models (LLMs) such as Meta Llama AI, Google Gemini AI, Anthropic Claude AI, or any other AI that offers a REST API.
- Use RecraftAI ImageProvider
- 
## Configuration

[More configuration](configuration.md)

## Dependencies

Episerver.CMS > 12.18
Episerver.Commerce > 14

## Discussions and feedback

This can be discussed in Github discussions (https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)

## Package maintainer

https://github.com/lucgosso

## TERMS OF USE

[License](license.md)
