# Azure Function for Implementing a Custom AI to the Epicweb AI-Assistant for Optimizely

This repository serves as a boilerplate template for integrating a Custom AI or custom implementation of the AI-Assistant within Optimizely. It allows you to incorporate custom logic and utilize your preferred Large Language Models (LLMs) such as Meta Llama AI, Google Gemini AI, Anthropic Claude AI, or any other AI that offers a REST API.

## How it works

The plugin in Optimizely is easily configured so it will talk to your Azure Function service where your company can implement any AI or any custom logic

## Configuration

To configure the AI integration in your Optimizely setup, include the following settings in your configuration file:


```
"AIAssistant": {
      "AccountName": "<epicweb accountname>",// Provided by Epicweb support
      "ApiKey": "<your key>", // Your API key for the Azure function
      "ServiceUrl": "https://<your azure instance>.azurewebsites.net/api/CustomAI", 
      "ProviderName": "CustomAI",
      "CustomJson": "{\"prop\":\"fromAppsettings\"}",// Any custom data you want to send to your API
      }
```

## Installation

Implement your custom logic in the CustomAIFunction.cs file and publish your Azure function according to the Azure documentation https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-function.

## Local debugging

Localhost addresses are not supported. To enable local debugging:

1. Create a free developer account at ngrok.

2. Download and install ngrok on your local machine.

3. Run the following command in your terminal to start ngrok:

```c:/pathto/ngrokfolder ngrok http 7094 -host-header="localhost:7094"```

4. Replace Epicweb.ServiceUrl with the new ngrok URL provided after running the above command.

## Data Fields

Here are the key data fields that you can access and manipulate within your custom AI function:

data.Prompt //The processed prompt.
data.Instructions //Instructions provided to the AI.
data.SystemPrompt //Additional instructions, rules, or terms for the AI.
data.Action //The requested action, which can be "default", "vision", or "imagegeneration".
data.Base64Image //A supplied image in Base64 format.

data.OriginalRequest //The original request from the plugin before processing by Epicweb.
data.OriginalRequest.Text //The original text/prompt from the plugin.
data.OriginalRequest.Prompt //Processed prompt from the plugin.
data.OriginalRequest.Shortcut //Name of the shortcut, if any
data.OriginalRequest.CustomJson //A string that needs to be serialized.
data.OriginalRequest.IsAuto //Indicates if the request is from an automatic process in Optimizely (e.g., LanguageManager).
data.OriginalRequest.IsHtml //if true use HTML in response, probably used in Rich Text Editor
data.OriginalRequest.CorrelationId //Used for support or troubleshooting along with the timestamp.
data.OriginalRequest.Lang // The language context of the request.

## support

Contact support @ optimizely.guru for demo or support

## Get Started with Epicweb AI-Assistant for Optimizely

Fill in the form on https://aiassistant.optimizely.blog/