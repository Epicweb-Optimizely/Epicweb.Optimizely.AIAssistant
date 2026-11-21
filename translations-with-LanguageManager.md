# Information about the Epicweb.Optimizely.AIAssistant.LanguageManager Package

Blog posts: https://optimizely.blog/?tag=Translations

Install versions compatible with your CMS and .NET target. The integration package declares these requirements.

| Optimizely CMS | .NET target | EPiServer.Labs.LanguageManager |
| --- | --- | --- |
| 12.18 or later in the 12.x series | .NET 8 | 5.1.4 or later in the 5.x series |
| 13.1 or later in the 13.x series | .NET 10 | 6.x |


- [EPiServer.Labs.LanguageManager](https://nuget.optimizely.com/packages/EPiServer.Labs.LanguageManager/)
- [Epicweb.Optimizely.AIAssistant.LanguageManager](https://nuget.optimizely.com/packages/epicweb.optimizely.aiassistant.languagemanager/)

Register the integration in `Startup.cs` or `Program.cs` after [setting up AI Assistant](installation.md). CMS 13 applications also register LanguageManager with `services.AddLanguageManager()`.

```csharp
using Epicweb.Optimizely.AIAssistant.LanguageManager;

services.AddLanguageManagerAIAssistant();
```

In the CMS Language Manager settings, select **AI-Assistant** as the translation provider. It uses the AI Assistant configuration, so leave the LanguageManager subscription key and region fields empty.

![image](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/3f2910ce-7f90-4e29-aef7-27d9a58a7b0a)

Use the LanguageManager gadget to translate a page. See the [AI Assistant license information](license.md) for subscription terms.

![image](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/b84e5b60-d75c-4d2e-ab0f-1e2edfd570a6)

## Configure Translations

Read [the configuration for translations here](configuration-translations.md)

## This is how it works (Video):

https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/aea7c323-47f7-45c1-93a9-9185e5074e0f

