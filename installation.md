# Installation Epicweb.Optimizely.AIAssistant

The installation should be done by a developer who has access to the source code of your solution.

Install it thru nuget package management:  https://nuget.optimizely.com/package/?id=Epicweb.Optimizely.AIAssistant

For property-level assistance, register AI Assistant and enable it on the properties or TinyMCE toolbars you want to use.

1. Add to startup.cs: ```services.AddAIAssistant(); ```

2. Use `[UIHint(AIHint.Input)]` or `[UIHint(AIHint.Textarea)]` for `string` properties. Use `[UIHint(AIHint.Image)]` for image properties of type `ContentReference` or `Url`.

3. Decorate properties with [AIUseToAnalyzeContent] - used to analyze a page or block for important content if referred - add this only on properties that give meaning for the context

4. Add "ai_assistant_execute ai_assistant_choices ai_assistant_image" to Toolbar in TinyMceConfiguration

5. Use the [`AIAssistant` attribute](configuration.md#aiassistant-attribute) to configure properties and supply instructions.

6. For image generation, configure the [image provider and its credentials](configuration.md#image-generation).

7. For global instructions, see [`IAssistantInstructionsResolver`](configuration.md#iassistantinstructionsresolver) and [embedded instructions](chat-instructions.md#customizing-chat-instructions).

8. For Analyzing Images use [AnalyzeImageAltText], [AnalyzeImage] on imagedata models properties and add ```app.AddImageAnalyzer();``` in startup.

9. **[RECOMMENDED]** Install the built in tools, download package => https://nuget.optimizely.com/package/?id=Epicweb.Optimizely.AIAssistant.Tools 

Register the tools in `Startup.cs`:
```csharp
using Epicweb.Optimizely.AIAssistant;
using Epicweb.Optimizely.AIAssistant.Tools;

services
   .AddAIAssistant()
   // Register MCP tool types for AI Assistant
   //.AddEmbeddedInstructionStore()//only if added your own instructions, otherwise not needed read more "chat-instructions.md"
   .RegisterMcpToolType(typeof(BuiltinTools))
   .RegisterMcpToolType(typeof(BuiltinChatTools)) 
   .RegisterMcpToolType(typeof(BuiltinPublishChatTools))
   .RegisterMcpToolType(typeof(BuiltinUpdateChatTools))   
   .RegisterMcpToolType(typeof(BuiltinCreateChatTools))
   .RegisterMcpToolType(typeof(VisitorGroupTools))  // Visitor group discovery for AI context
   .RegisterMcpToolType(typeof(DisplayOptionTools))// Use this to understand what display options are available for ContentArea items.
   .RegisterMcpToolType(typeof(BuiltinChatImageTools)) //only together with image analysation module (licensed)
   .RegisterMcpToolType(typeof(BuiltinChatCreateImageTools));//only Image Creation Instructions, only together with image generation module (licensed)
```

Register the built-in tools for the CMS operations you want chat to perform, such as reading content, editing pages, publishing, or analysing images.

10. To enable AI Chat, add these settings to `appsettings.json`.
```json
{
  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-...",
      "EnableChat": true,
      "ChatRoles": ["AIEditors", "WebAdmins"],
      "MaxToolIterations": 8
    }
  }
}
```

**Configuration Options:**
- `EnableChat` - (bool) Enables the AI Chat window in CMS toolbar. Default: `false`
- `ChatRoles` filters who sees AI Chat in the CMS editor. The list is unset by default. The CMS chat endpoints require one of `AIEditors`, `WebEditors`, `WebAdmins`, `CmsAdmins`, or `CmsEditors`.
- `MaxToolIterations` - (int) Maximum number of tool call iterations to prevent infinite loops. Default: `8`

**IMPORTANT:** When `EnableChat` is `true`, you must also register the endpoints.MapAIAssistantHub hub in your `Startup.cs`:
Use a provider and model that support tool calling for chat workflows that use tools.

```csharp
using Epicweb.Optimizely.AIAssistant;
using Epicweb.Optimizely.AIAssistant.Tools;

public void ConfigureServices(IServiceCollection services)
{    
    services
        .AddAIAssistant()
        .RegisterMcpToolType(typeof(BuiltinChatTools));
        // ... other registrations
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ... other middleware
    
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapContent();
        endpoints.MapAIAssistantHub(); // Required when Chat is enabled
    });
}
```

**Note:** The SignalR hub (`MapAIAssistantHub()`) is required for AI Chat to function properly. It enables real-time communication between the chat interface and the backend AI service.


For a free evaluation without any licensing messages in the production environment, please complete the form at https://aiassistant.optimizely.blog

## AI Chat Feature

The AI Chat feature provides a conversational interface for content editors to interact with AI directly in the CMS.

### Getting Started with AI Chat

1. **Enable Chat** in `appsettings.json` (see step 10 above)
2. **Register Tools** for the CMS operations you want to enable
3. **Configure Roles** - Specify which user roles can access the chat
4. **Customize Instructions** - Optional: Add custom chat instructions (see below)

### Quick Start Guide

For detailed information on using AI Chat, see:
- [AI Chat - Getting Started Guide](chat-instructions.md) - Complete guide to using the chat feature
- [AI Tools - Function Calling and MCP](tools.md) - Understanding and creating custom tools

### Chat Configuration in appsettings.json

Full configuration example:
```json
{
  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-...",
      "ProviderName": "OpenAI",
      "AIModel": "gpt-5.2",
      "EnableChat": true,
      "ChatRoles": ["AIEditors", "WebAdmins"],
      "MaxToolIterations": 8
    }
  }
}
```

### Custom Chat Instructions

You can customize AI Chat behavior by creating instruction files:

1. Add the relevant [instruction files](chat-instructions.md#customizing-chat-instructions). For example, `Instructions/Chat/Global.md` applies to chat without a content context.
2. Mark the Markdown files as embedded resources in `.csproj`.
3. Register the application assembly with `AddEmbeddedInstructionStore()`.

```csharp
services.AddAIAssistant()
       .AddEmbeddedInstructionStore(
           Assembly.GetExecutingAssembly(),
           priority: 20
       );
```

See [Chat Instructions Guide](chat-instructions.md) for more details.

## Administration and SEO Manager access

Assign `AIEditors` or `WebAdmins` to give a user access to the AI Assistant administration pages. To give another CMS role access to SEO Manager, enable `EnableSeoManager` and add the role to `AIAssistantSEOManager` under `Epicweb:AIAssistant` in `appsettings.json`. See [admin roles and configuration examples](admin-features.md#accessing-administration).

## Further Configurations

[More configuration](configuration.md) - General configuration options

[AI Tools - Function Calling and MCP](tools.md) - Complete guide to AI tools, including creating custom tools

[Configuration translations](configuration-translations.md) - Translation settings

[Configuration Image Analyzer](configuration-image-analyzer.md) - Image analysis setup

[AI Chat Instructions](chat-instructions.md) - Complete chat feature guide

## Dependencies

The NuGet package has separate builds for each CMS major version.

| Optimizely CMS | .NET target |
| --- | --- |
| 12.18 or later in the 12.x series | .NET 8 |
| 13.x | .NET 10 |

## Discussions and feedback

This can be discussed in Github discussions (https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)

## Package maintainer

https://github.com/lucgosso

## TERMS OF USE

[License](license.md)
