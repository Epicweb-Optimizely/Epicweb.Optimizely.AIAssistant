# Azure Function for a custom AI provider

This sample shows the CustomAI request and response contracts using an Azure Functions v4 .NET 8 isolated worker. Implement your own model call or application logic in [CustomAIFunction.cs](CustomAIFunction.cs).

## How it works

The AI Assistant plugin sends the `CustomAI` provider and `ServiceUrl` to the Epicweb AI service. The sample receives a JSON POST at `/api/CustomAI` and returns a response based on the requested action.

## Configuration

Add these settings to the Optimizely application. Set the actual credential through your application's secret configuration.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "AccountName": "<epicweb-account-name>",
      "ApiKey": "<custom-provider-credential>",
      "ServiceUrl": "https://<function-app>.azurewebsites.net/api/CustomAI",
      "ProviderName": "CustomAI",
      "CustomJson": "{\"prop\":\"fromAppsettings\"}"
    }
  }
}
```

`CustomJson` is optional. The CMS plugin sends it as a string, which your function can parse if it contains JSON.

## Installation

Implement the `default`, `vision`, or `imagegeneration` actions that your integration needs before deploying the function. The sample uses an anonymous HTTP trigger and only checks whether the Authorization header has a value. Implement credential validation for your endpoint before publishing it.

## Local debugging

The launch profile uses port `7094`. To test the function itself, run it locally and send a POST to `http://localhost:7094/api/CustomAI`. To test the Optimizely integration, set `Epicweb:AIAssistant:ServiceUrl` to a full HTTPS endpoint including `/api/CustomAI` that the configured AI service can reach.

## Data Fields

[AIRequest.cs](AIRequest.cs) defines the incoming JSON contract. The `prompt`, `instructions`, and `systemPrompt` fields carry prepared input for the model. `originalRequest` can include the CMS input (`text`), language (`lang`), HTML flag (`isHtml`), `customJson`, and `correlationId` for tracing.

| Action | Input | Sample response |
|--------|-------|-----------------|
| `default` | Text and prompt data | [AIResult](AIResult.cs) with text and token usage |
| `vision` | Prompt and `base64Image` | [AIResult](AIResult.cs) with analysis text |
| `imagegeneration` | The image prompt | Array of [ImageResult](ImageResult.cs) objects with a URL or base64 image |

## Support

Contact support@optimizely.guru or visit the [Epicweb AI Assistant website](https://aiassistant.optimizely.blog/).
