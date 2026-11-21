# Bring your own AI to the AI-Assistant

Use the `CustomAI` provider to route AI Assistant requests to a service you operate. That service can call a language model or implement your own processing.

## How it works

The Optimizely plugin sends its request to the Epicweb AI service, including the configured provider, `ServiceUrl`, and any `CustomJson`. The Custom AI endpoint specified by `ServiceUrl` must be reachable from the AI service.

## Dev documentation

The [Azure Function sample](src/Epicweb.Azure.Function.CustomAI/readme.md) shows the request and response contracts and how to set `Epicweb:AIAssistant:ProviderName` to `CustomAI`.