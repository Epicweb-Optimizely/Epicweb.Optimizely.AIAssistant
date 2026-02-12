# Installation Epicweb.Optimizely.AIAssistant

The installation should be done by a developer who has access to the source code of your solution.

Install it thru nuget package management:  https://nuget.optimizely.com/package/?id=Epicweb.Optimizely.AIAssistant

For demo and evaluation, the only required configuration is to include the **AI**HINT attribute for the property.

1. Add to startup.cs: ```services.AddAIAssistant(); ```

2. Decorate your string property with [UIHint(AIHint.Image)], [UIHint(AIHint.Textarea)] or [UIHint(AIHint.Input)] for single line strings

3. Decorate properties with [AIUseToAnalyzeContent] - used to analyze a page or block for important content if referred - add this only on properties that give meaning for the context

4. Add "ai_assistant_execute ai_assistant_choices ai_assistant_image" to Toolbar in TinyMceConfiguration

5. Use [AIAssistant]-attribute to configure properties and give assistant instruction => https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/blob/master/configuration.md#aiassistant

6. For image generations to work, you need to register an account and create a api-key https://platform.openai.com/account/api-keys => then add it to appsettings.json "Epicweb": { "AIAssistant": { "ApiKey": "sk-4ks...." }}

7. For global instructions to AI Assistant => implement IAssistantInstructionsResolver => https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/blob/master/configuration.md#iassistantinstructionsresolver

8. For Analyzing Images use [AnalyzeImageAltText], [AnalyzeImage] on imagedata models properties and add ```app.AddImageAnalyzer();``` in startup.

9. **[RECOMMENDED]** Install the built in tools, download package => https://nuget.optimizely.com/package/?id=Epicweb.Optimizely.AIAssistant.Tools 

Register the tools in `Startup.cs`:
```csharp
using Epicweb.Optimizely.AIAssistant.Tools;

services
   .AddAIAssistant()
   // Register MCP tool types for AI Assistant
   //.AddEmbeddedInstructionStore()//only if added your own instructions, otherwise not needed read more "chat-instructions.md"
   .RegisterMcpToolType(typeof(BuiltinTools))
   .RegisterMcpToolType(typeof(BuiltinChatTools))
   .RegisterMcpToolType(typeof(BuiltinPublishChatTools))
   .RegisterMcpToolType(typeof(BuiltinUpdateChatTools))
   .RegisterMcpToolType(typeof(BuiltinChatImageTools))
   .RegisterMcpToolType(typeof(BuiltinChatCreateImageTools));//only Image Creation Instructions


   // ... other registrations
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapContent();
        endpoints.MapAIAssistantHub(); // Required when Chat is enabled
    });
```

**Note:** Built-in tools are essential for AI Chat functionality and enable features like content reading, updates, publishing, and image operations.

10. **[NEW in 3.0]** Enable AI Chat Window - Add to `appsettings.json`:
```json
{
  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-...",
      "EnableChat": true,
      "ChatRoles": ["AIEditors", "WebEditors", "WebAdmins", "CmsAdmins", "CmsEditors"], // Roles that can access the chat
      "MaxToolIterations": 8 // Prevent infinite loops in tool calling
    }
  }
}
```

**Configuration Options:**
- `EnableChat` - (bool) Enables the AI Chat window in CMS toolbar. Default: `false`
- `ChatRoles` - (string[]) User roles that have access to AI Chat. Default: `["AIEditors", "WebEditors", "WebAdmins", "CmsAdmins", "CmsEditors"]`
- `MaxToolIterations` - (int) Maximum number of tool call iterations to prevent infinite loops. Default: `8`

**IMPORTANT:** When `EnableChat` is `true`, you must also register the endpoints.MapAIAssistantHub hub in your `Startup.cs`:
**IMPORTANT:** With the chat it is important to use the right model, do not use GPT-4 legacy models nor Gemini flash 1.5

```csharp
using Epicweb.Optimizely.AIAssistant.Hub;

public void ConfigureServices(IServiceCollection services)
{    
    services
        .AddAIAssistant()
        .RegisterMcpToolType(typeof(BuiltinChatTools))
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

## AI Chat Feature (New in 3.0)

The AI Chat feature provides a conversational interface for content editors to interact with AI directly in the CMS.

### Getting Started with AI Chat

1. **Enable Chat** in `appsettings.json` (see step 10 above)
2. **Register Tools** - Chat requires tools to be registered for full functionality
3. **Configure Roles** - Specify which user roles can access the chat
4. **Customize Instructions** - Optional: Add custom chat instructions (see below)

### Quick Start Guide

For detailed information on using AI Chat, see:
- [AI Chat - Getting Started Guide](chat-instructions.md) - Complete guide to using the chat feature
- [AI Tools - Function Calling and MCP](Tools.md) - Understanding and creating custom tools

### Chat Configuration in appsettings.json

Full configuration example:
```json
{
  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-...",
      "Provider": "OpenAI",
      "Model": "gpt-5.2",
      "EnableChat": true,
      "ChatRoles": ["AIEditors", "WebEditors", "WebAdmins", "CmsAdmins", "CmsEditors"], // Roles that can access the chat
      "MaxToolIterations": 8 // Prevent infinite loops in tool calling
    }
  }
}
```

### Custom Chat Instructions

You can customize AI Chat behavior by creating instruction files:

1. Create `Instructions/chat/` folder in your project
2. Add markdown files (e.g., `global.md`, `general.md`)
3. Mark as Embedded Resource in `.csproj`
4. Register in `Startup.cs`:

```csharp
services.AddAIAssistant()
       .AddEmbeddedInstructionStore(
           Assembly.GetExecutingAssembly(),
           priority: 20
       );
```

See [Chat Instructions Guide](chat-instructions.md) for more details.

## Further Configurations

[More configuration](configuration.md) - General configuration options

[AI Tools - Function Calling and MCP](Tools.md) - Complete guide to AI tools, including creating custom tools

[Configuration translations](configuration-translations.md) - Translation settings

[Configuration Image Analyzer](configuration-image-analyzer.md) - Image analysis setup

[AI Chat Instructions](chat-instructions.md) - Complete chat feature guide

## Dependencies

Episerver.CMS > 12.18
Episerver.Commerce > 14

## Discussions and feedback

This can be discussed in Github discussions (https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)

## Package maintainer

https://github.com/lucgosso

## TERMS OF USE

[License](license.md)
