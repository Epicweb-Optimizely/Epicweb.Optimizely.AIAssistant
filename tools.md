# AI Tools - Function Calling and MCP

AI tools let AI Assistant call registered .NET methods. Use them to fetch CMS context, access other systems, or perform content operations.

## Quick Start

### Installation

Install the tools package in your Optimizely CMS application.

```bash
dotnet add package Epicweb.Optimizely.AIAssistant.Tools
```

### Basic setup

Register the built-in tool classes that your application needs in `ConfigureServices` or equivalent startup code.

```csharp
using Epicweb.Optimizely.AIAssistant;
using Epicweb.Optimizely.AIAssistant.Tools;

services.AddAIAssistant()
    // Register built-in MCP tool types
    .RegisterMcpToolType(typeof(BuiltinTools))               // Content lookup and querying
    .RegisterMcpToolType(typeof(BuiltinChatTools))           // Content reading and management
    .RegisterMcpToolType(typeof(BuiltinPublishChatTools))    // Publishing and versioning
    .RegisterMcpToolType(typeof(BuiltinUpdateChatTools))     // Content property updates
    .RegisterMcpToolType(typeof(BuiltinCreateChatTools))     // Content discovery and creation
    .RegisterMcpToolType(typeof(CategoryTools))              // Category discovery and management
    .RegisterMcpToolType(typeof(DisplayOptionTools))         // Display option discovery
    .RegisterMcpToolType(typeof(VisitorGroupTools))          // Visitor group and personalization
    .RegisterMcpToolType(typeof(BuiltinChatImageTools))      // Image analysis (requires license)
    .RegisterMcpToolType(typeof(BuiltinChatCreateImageTools));// Image generation (requires license)
```

To use the chat tools, enable [AI Chat](chat-instructions.md). Register `BuiltinChatTools` to make the `ListAvailableToolPacks` and `LoadToolPack` tools available in chat.

## Built-in tools

`BuiltinTools` exposes `GetContentById` and `GetThisContent` in field editors. Its chat lookup tools are grouped in the `ContentLookup` pack.

| Chat pack ID | Class | Tools |
| --- | --- | --- |
| `ContentLookup` | `BuiltinTools` | `GetChildContent`, `GetContentByName`, `FindByUrl`, `GetLatestPublishedContent` |
| `ContentDetails` | `BuiltinChatTools` | `GetContentProperties`, `GetIContentVersions` |
| `SiteInfo` | `BuiltinChatTools` | `GetSiteLanguages`, `GetStartPage` |
| `UITools` | `BuiltinChatTools`, `DisplayOptionTools` | `OpenContentVersion`, `GetAvailableDisplayOptions` |
| `Update` | `BuiltinUpdateChatTools`, `CategoryTools` | `UpdateMultiplePropertiesOnPageOrBlock`, `SetContentCategories`, `AddContentCategories`, `RemoveContentCategories` |
| `Publish` | `BuiltinPublishChatTools` | `PublishOrUnpublishContent`, `CheckScheduledPublish`, `CreateLanguageVersion` |
| `Create` | `BuiltinCreateChatTools` | `GetAvailablePageTypes`, `GetAvailableBlockTypes`, `GetAssetFolderInfo`, `GetContentTypeSchema`, `CreatePage`, `CreateBlock` |
| `Image` | `BuiltinChatImageTools`, `BuiltinChatCreateImageTools` | `AskImageQuestions`, `GetImageProperties`, `ImageCreationInstructions` |
| `Categories` | `CategoryTools` | `GetAllCategories`, `GetCategory`, `SearchCategories`, `GetCategoryHierarchy`, `GetContentCategories`, `GetCategoriesByParent` |
| `VisitorGroups` | `VisitorGroupTools` | `GetAllVisitorGroups`, `GetVisitorGroup`, `GetVisitorGroupRoles`, `SearchVisitorGroups` |
| `AssistantInstructions` | `BuiltinChatTools` | `GetTranslationInstructions`, `GetAIInstructions`, `GetAIInstruction` |
| `AssistantManagement` | `BuiltinChatTools` | `CreateAssistant`, `GetAssistants`, `UpdateAssistant` |
| `UserAPIInstructionsToolPack` | `BuiltinChatTools` | `CreateQuickInstruction`, `GetQuickInstructionsForUsers`, `UpdateQuickInstruction` |

Register the classes in the table to expose their tools. Image questions use the configured [image analyzer](configuration-image-analyzer.md). `ImageCreationInstructions` directs editors to the **AI Image Editor**, where image generation takes place.

## Tool scopes

Use `[ToolScope(ToolScope.Fields)]` for a tool intended for property editors and `[ToolScope(ToolScope.Chat)]` for a tool intended for chat. `ToolScope.All` makes a tool eligible for both. Methods without a scope attribute default to `All`.

## Creating custom tools

Mark a class and its methods with the MCP attributes. Use `Description` on methods and parameters to explain when to call a tool and what input it expects. Use `RequestModel` when a tool needs the current CMS content ID, language, or property. AI Assistant supplies this parameter and leaves it out of the model-facing tool schema.

```csharp
using System.ComponentModel;
using Epicweb.Optimizely.AIAssistant;
using Epicweb.Optimizely.AIAssistant.Attributes;
using Epicweb.Optimizely.AIAssistant.Models;

[McpServerToolType]
public static class EditorialTools
{
    [McpServerTool(Name = "GetEditingContext")]
    [ToolScope(ToolScope.Chat)]
    [Description("Return the current CMS content ID and language.")]
    public static object GetEditingContext(RequestModel context = null)
    {
        return new
        {
            contentId = context?.ContentId,
            language = context?.Lang
        };
    }
}
```

Once your application has called `AddAIAssistant()`, register the class in startup.

```csharp
services.RegisterMcpToolType(typeof(EditorialTools));
```

### Async tools and complex parameters

Tools can use async methods for I/O. The following example calls a catalogue API chosen by the application. It expects a JSON product at `products/{sku}?language={language}`. The model supplies the SKU and language while the application supplies the destination and any credentials.

```csharp
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Epicweb.Optimizely.AIAssistant;
using Epicweb.Optimizely.AIAssistant.Attributes;
using EPiServer.ServiceLocation;

[McpServerToolType]
public static class ProductCatalogTools
{
    [McpServerTool(Name = "GetProductDetails")]
    [ToolScope(ToolScope.Chat)]
    [Description("Get a product name and description from the catalogue.")]
    public static async Task<object> GetProductDetails(
        [Description("Object with sku (1–40 ASCII letters, digits, hyphens, or underscores) and language (en or sv).")]
        ProductLookup lookup)
    {
        if (lookup == null || string.IsNullOrWhiteSpace(lookup.Sku) ||
            lookup.Sku.Length > 40 || !lookup.Sku.All(character => char.IsAsciiLetterOrDigit(character) || character is '-' or '_') ||
            lookup.Language is not ("en" or "sv"))
        {
            return new { error = "Provide a SKU of 1–40 ASCII letters, digits, hyphens, or underscores and choose en or sv." };
        }

        var factory = ServiceLocator.Current.GetInstance<IHttpClientFactory>();
        using var client = factory.CreateClient("ProductCatalog");
        using var response = await client.GetAsync(
            $"products/{Uri.EscapeDataString(lookup.Sku)}?language={lookup.Language}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new { error = "Product not found.", sku = lookup.Sku };
        }

        response.EnsureSuccessStatusCode();
        var product = await response.Content.ReadFromJsonAsync<ProductDetails>();
        if (product == null)
        {
            return new { error = "The catalogue returned no product data." };
        }

        return product;
    }
}

public sealed class ProductLookup
{
    public string Sku { get; set; } = string.Empty;
    public string Language { get; set; } = "en";
}

public sealed class ProductDetails
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```

In startup, configure the named client and register the tool class. Use a trusted catalogue URL with a trailing slash.

```csharp
services.AddHttpClient("ProductCatalog", client =>
{
    client.BaseAddress = new Uri(configuration["ProductCatalog:BaseUrl"]);
    client.Timeout = TimeSpan.FromSeconds(10);
});
services.RegisterMcpToolType(typeof(ProductCatalogTools));
```

The generated tool schema describes each top-level parameter and marks parameters without default values as required. For a class such as `ProductLookup`, it describes the parameter as an object without listing its nested properties. Describe the expected fields and limits with `Description` and validate them in the method. For a complex input that needs a precise text format, a tool can also accept a JSON string and deserialize it explicitly, as `UpdateMultiplePropertiesOnPageOrBlock` does with `propertiesJson`.

### Permissions and errors

Check the current user's CMS access before returning content or writing changes. Built-in tools illustrate both paths. `GetContentProperties` checks read access, and `UpdateMultiplePropertiesOnPageOrBlock` checks edit access and passes `AccessLevel.Edit` when saving. Ask editors to confirm proposed updates before saving. Favour structured error results for expected problems such as missing items or invalid input. The executor logs thrown exceptions and includes their messages in tool output, so keep credentials and private diagnostics out of those messages.

### Tools in fields

Open **Ask AI Assistant** for an AI-enabled property and type `@` to see tools available in field scope. Choose a tool to insert its name in the prompt.

A custom [shortcut](promptshortcuts.md#using-tools-with-shortcuts) can request particular tool classes with `IPromptShortcut.Tools` and opt into execution with `UseTools`. For a content type in chat, `AIInstructionsAttribute.Tools` accepts tool classes applied when the item becomes context or is attached.

## Tool packs

In chat, the default tools are registered methods that match the chat scope and have no pack ID. Ask AI Assistant to list available packs or to load one by its ID. For example, ask it to load `ContentDetails` before retrieving a page's property metadata. The assistant can also request a relevant pack when it needs tools that are not loaded.

### Creating a custom tool pack

Add the pack ID to a tool's scope attribute, for example `[ToolScope(ToolScope.Chat, "Editorial")]`, and register the tool class. To supply additional guidance when listing the pack, register an `IToolPack` with the same ID.

```csharp
using Epicweb.Optimizely.AIAssistant;

public sealed class EditorialToolPack : IToolPack
{
    public string Id => "Editorial";
    public ToolScope Scope => ToolScope.Chat;
    public string Instructions => "Use GetEditingContext to inspect the current CMS item.";
}
```

```csharp
services.AddSingleton<IToolPack, EditorialToolPack>();
```

## Context vs tools

When you start chat with a page, the chat includes CMS context for that item. Use tools to retrieve the properties or related content the task needs.

## Tool execution flow

For example, an editor can ask chat to propose a new SEO title for the current page. The assistant can load `ContentDetails` and call `GetContentProperties` to find the property's actual name and read the current metadata. After confirmation, it can load `Update` and call `UpdateMultiplePropertiesOnPageOrBlock` with JSON like this for a site that stores its title in `MetaTitle`.

```json
{
  "contentReference": "this",
  "propertiesJson": "[{\"propertyName\":\"MetaTitle\",\"value\":\"Sustainable products | Example Store\"}]",
  "language": "en"
}
```

`PublishOrUnpublishContent` is a separate tool and supports scheduled publication.

## Property JSON Converters

`GetContentProperties` exposes converter instructions and schema for supported properties. The content creation and update tools use the same converter registry when assigning values. See [Property JSON Converters](property-converters.md) for built-in formats and registration.

## Testing tools

Call a custom tool method directly with representative valid and invalid inputs. Supply a test `RequestModel` for methods that require context. For HTTP tools, test the application client against a stub response for success and missing data.

In the CMS, verify that the tool appears for the intended scope and can run with the intended user's permissions. For a grouped tool, ask chat to list the packs, load the intended pack, and run the tool. In a property editor, check that the prompt dialog offers the expected field tool in its `@` list and try a prompt that uses it.

Use **Display system messages** to inspect tool output in chat, or turn on **Console Debug Mode** in chat preferences for browser logs. Check the server logs for the call name and for execution or argument errors.

## Real-world examples

The Alloy sample has an asynchronous lookup in `Alloy/Tools/ProductInfoTools.cs` and its pack definition in `Alloy/ToolPacks/ProductToolPack.cs`. Register both to make `GetProductDescription` available under the `Product` pack. It returns sample data. To work with an actual catalogue, connect a custom tool to the site's product service.

## Demo video

[See AI Tools in action](https://github.com/user-attachments/assets/726e2cb3-11fe-4ef7-8bf9-fafa22a9a1c1)

## Troubleshooting

- When a tool is missing, check its class registration, method attribute, scope and pack ID. In chat, use `ListAvailableToolPacks` to verify that the pack is available.
- When a CMS tool returns the wrong content, check the selected item, language and version. Verify that the requesting user has access to the content.
- Enable **Console Debug Mode** in chat preferences to inspect browser-side requests and tool activity.

## Additional resources

- [Chat Instructions](chat-instructions.md)
- [Installation Guide](installation.md)
- [Configuration Guide](configuration.md)
- [Video tutorials](https://aiassistant.optimizely.blog/en/videos/)
- [GitHub Discussions](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)
