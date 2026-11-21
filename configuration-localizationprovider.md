# AI Assistant for DbLocalizationProvider

## Introduction
The LocalizationProvider is a powerful tool designed to enhance the localization capabilities of Optimizely CMS. It allows editors to manage translations efficiently and effectively, ensuring that applications can cater to a global audience.

You can find the project [here](https://github.com/valdisiljuconoks/LocalizationProvider/blob/master/README.md).

[localizationprovider-ai-assistant.webm](https://github.com/user-attachments/assets/f476a4d5-a2ce-4cc9-a91c-1bfe44e8787c)


## Connecting to AI-Assistant
With the integration of AI-Assistant, editors can now leverage cloud-based translation services to boost productivity and streamline the content translation process. This connection enables real-time translation capabilities, enhancing the overall localization process.

## Getting Started
To get started with the LocalizationProvider, download the package: `Epicweb.Optimizely.AIAssistant.DbLocalizationProvider` to your Optimizely CMS solution.

### Adding Services
To integrate the LocalizationProvider into your application, add the following service in your Startup configuration:

```csharp
using Epicweb.Optimizely.AIAssistant.DbLocalizationProvider;

services.AddAIAssistantDbLocalizationProvider();
```

### Dependencies

Register [AI Assistant](installation.md) and DbLocalizationProvider before adding the integration. It registers AI Assistant's translation service as the DbLocalizationProvider translator and uses the existing AI Assistant settings.

The package depends on `Epicweb.Optimizely.AIAssistant.LanguageManager`, which depends on the base AI Assistant package. The .NET 8 build uses `DbLocalizationProvider.AdminUI.EPiServer` 8.2.4 through 8.x. The .NET 10 build uses 9.x.
