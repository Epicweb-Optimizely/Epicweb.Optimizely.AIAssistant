# Configuration

## Attributes

### UIHint

**[UIHint(AIHint.Input)]** => for one line input fields (string)

**[UIHint(AIHint.Textarea)]** => for multi line input fields (string)

**[UIHint(AIHint.Image)]** => for image fields (ContentReference or Url)

### [AIUseToAnalyzeContent]

You can utilize prompts with the placeholder ::this::. This means it can comprehend and integrate with the context of the page, block, product, or any other object the user is currently engaging with.

To ensure this functions correctly, you should annotate the model's property with [AIUseToAnalyzeContent]. However, it's important not to apply it to every property—only those that are relevant to the context of the object.

### [AIAssistant]-attribute

#### For text fields
**[AIAssistant(Model = "gpt-4o-mini")]** => The model to use => https://platform.openai.com/docs/models 

For Azure OpenAI, use the deployment name.

**[AIAssistant(AssistantInstructions = "You are a selling professional sales assistant")]** => It's designed to set the behavior of the assistant. You might use the message to specify a role or provide some context for the assistant.

**[AIAssistant(MinimalMode = true)]** => Default is false, hides the "AI Wheel" by default

Set `[AIAssistant(AutoSuggest = true)]` to request a suggestion when the editor leaves an AI textarea. If the attribute omits `AutoSuggest`, the textarea uses the application setting, which defaults to `false`.

Set `[AIAssistant(ShortcutsDisabled = true)]` to hide shortcuts in the AI text editor for this property.

See [Shortcuts Guide](./promptshortcuts.md) for detailed information on available shortcuts, configuration, and creating custom shortcuts.

**[AIAssistant(CustomJson = "{\"prop\":\"fromAttribute\"}")]** => Any custom data you want to send to your API, to be used with CustomAI Provider

```json
{
  "Epicweb": {
    "AIAssistant": {
      "CustomJson": "{\"prop\":\"fromAppsettings\"}"
    }
  }
}
```

Use `[AIAssistant(Roles = new[] { "AIEditors", "SomeRole" })]` to restrict this property to specific roles. A nonempty property role list takes precedence over the application role list.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "Roles": ["AIEditors", "SomeRole"]
    }
  }
}
```

**[AIAssistant(Shortcuts = new[] {
        typeof(PromptShortcut), //prompt version 1.5
        typeof(SuggestPromptShortcut), //refine
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.TranslatePromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.ShortenPromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.ElaboratePromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.SummarizeShortPromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.SummarizePromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.ChangeTonePromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.HumorPromptShortcut),//you need to add all subprompts for "Change tone" if you want to use them
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.SeriousPromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.CheckSpellingPromptShortcut) })]** => Only specify the ones you want to use on this property. See the [Shortcuts Guide](promptshortcuts.md) for available options and configuration examples.

Register `SeoTitlePromptShortcut` to offer SEO title suggestions based on page content.
```csharp
services.AddSingleton<IPromptShortcut, SeoTitlePromptShortcut>();
```

#### For XHtmlString fields (Rich text editor)

**[AIAssistant(QueryParameters = "")]** => Additional querystring appended to imageurl into XHtmlString (TinyMCE) eg width=100&height=300&format=webp&quality=75

Use `[AIAssistant(ImageWidth = 1024)]` to set the `width` attribute on images inserted into the TinyMCE field when `QueryParameters` is unset.

**[AIAssistant(Shortcuts = new[] { ... })]** => Same as above for text fields. See the [Shortcuts Guide](promptshortcuts.md) for RTE-specific shortcuts and configuration options.

TinyMCE uses the same [image generation settings](#image-properties) as image fields.

#### For Image fields (ContentReference or Url)

Select the editor with `[UIHint(AIHint.Image)]`. Use the [image generation settings](#image-properties) to customize model, size, or style.

## Appsettings

### Administration and SEO Manager access

Administration pages use the `AIAssistantAdminAccess` policy. Under `Epicweb:AIAssistant`, `Roles` controls access to field assistance and `ChatRoles` controls visibility of AI Chat in the CMS editor.

| Setting under `Epicweb:AIAssistant` | Default | Effect |
| --- | --- | --- |
| `EnableSeoManager` | `false` | Enables the SEO Manager page and API. |
| `AIAssistantSEOManager` | `[]` | Adds CMS roles that can open SEO Manager when it is enabled. `AIEditors` and `WebAdmins` already qualify. |

See [Granting SEO Manager access](admin-features.md#granting-seo-manager-access) for the JSON configuration and [Content permissions](seo-manager.md#content-permissions) for the rights on individual CMS pages.

### Configure AutoSuggest

`AutoSuggest` defaults to `false`. Set it to `true` to enable suggestions on blur across eligible fields. A property can override the application setting with `[AIAssistant(AutoSuggest = false)]`.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "AutoSuggest": true
    }
  }
}
```
### Enable Minimal Mode

Minimal mode hides the AI wheel initially. Enable it per field with `[AIAssistant(MinimalMode = true)]` or for all eligible fields in `appsettings.json`. The default is `false`.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "MinimalMode": true
    }
  }
}
```

### Disable AI Assistant on selected sites

`DisabledSites` accepts site IDs and site names.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "DisabledSites": ["025c64b0-c66d-4d75-8606-92c2866cb555", "MySiteName"]
    }
  }
}
```

## TinyMCE Rich Text Editor (XHtmlString-properties)

Add "ai_assistant_execute ai_assistant_choices ai_assistant_image" to Toolbar in TinyMceConfiguration

Use `services.AddAIAssistant(addToDefaultTinyMCE: false)` to skip automatic registration on the default TinyMCE configuration.

To register the plugin on a custom TinyMCE configuration, call `AddAIAssistantToTinyMCE()` on its `TinyMceSettings` and add the AI toolbar entries.

## Image Generation

Add [UIHint(AIHint.Image)] for image fields (ContentReference or Url)

Add "ai_assistant_image" to Toolbar in TinyMceConfiguration

The default folder resolver uses the content item's asset folder when available and otherwise uses the site's global asset root. To customize this behavior, register your own `IFolderResolver` after `AddAIAssistant()`.

### Modifying File Names and Image Descriptions

Register your own `IMediaPropertyResolverResolver` after `AddAIAssistant()` to modify generated media before saving. The hook can update the `Description`, `Name`, or image binary data.

### Image provider credentials

Set the credentials for your configured image provider. The plugin sends `ApiKey` and also supports a separate image key via `ApiKeyImage`. Use `ImageProviderName` and `ServiceUrlImage` for a separate provider or image endpoint. The example supplies a separate key for OpenAI image generation.
```json
{
  "Epicweb": {
    "AIAssistant": {
      "ImageProviderName": "OpenAI",
      "ApiKeyImage": "<image-provider-key>"
    }
  }
}
```

### Image Properties

Set `ImageModel` explicitly for your provider. This example uses `gpt-image-2` with OpenAI. For Azure OpenAI, use the deployed image model's deployment name. The CMS plugin still sends the legacy `dall-e-3` identifier when no image model is configured. See the [change log](change-log.md) for image model support by release.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "ImageProviderName": "OpenAI",
      "ImageModel": "gpt-image-2"
    }
  }
}
```

The `AIAssistant` attribute can override `ImageModel` and specify `ImageGenerationSize` or `ImageGenerationStyle` per property. Sizes and styles are provider-specific. Use `ImageStyle` to set an application-wide style where the provider supports it.

[Recraft Image Generation Configuration](recraftai-image-generation.md)

## Override default behaviors

You can override these implementations if you want to implement different logic

### IAssistantInstructionsResolver

The default resolver reads general instructions from `Instructions/General/Default.md` and translation instructions from `Instructions/Translation/General.md` when those resources are registered. Each lookup falls back to the `AssistantInstructions` property on the start page in the requested language if the instruction stores have no match. See [registering embedded instructions](chat-instructions.md#customizing-chat-instructions).

Implement `IAssistantInstructionsResolver` if you need to resolve instructions programmatically. The chat method has a default implementation that calls `GetInstructions(language)`.

```csharp
public interface IAssistantInstructionsResolver
{
    string GetInstructions(CultureInfo language);
    string GetInstructionsForTranslations(CultureInfo toLanguage, CultureInfo? fromLanguage = null);
    string GetInstructionsForChat(CultureInfo language, string mode = "General")
        => GetInstructions(language);
}
```

Register the implementation after `AddAIAssistant()`.

```csharp
public string GetInstructions(CultureInfo language)
{
    ContentReference start = ContentReference.StartPage;
    if (contentLoader.TryGet(start, language, out PageData startpage) && startpage.Property.TryGetPropertyValue("AssistantInstructions", out string value))
    {
        return value;
    }
    return configuration["Epicweb:AIAssistant:AssistantInstructions"];
}

public string GetInstructionsForTranslations(CultureInfo toLanguage, CultureInfo? fromLanguage = null)
{
    ContentReference start = ContentReference.StartPage;
    if (contentLoader.TryGet(start, toLanguage, out PageData startpage) && startpage.Property.TryGetPropertyValue("AssistantInstructions", out string value))
    {
        return value;
    }
    return configuration["Epicweb:AIAssistant:AssistantTranslationInstructions"];
}
```
### IAITinyMceTemplateResolver

Override the IAITinyMceTemplateResolver to implement your own image-tag implementation in the rich text editor

```csharp
public interface IAITinyMceTemplateResolver
{
    /// <summary>
    /// By overriding, building a custom html image template to AI-Assistant insert in tinymce
    /// </summary>
    /// <param name="contentReference">What image to use</param>
    /// <param name="insertingToContentReference">if you want to do some custom html depending on type of block or page</param>
    /// <param name="propname">The property on IContent used</param>
    /// <param name="lang">The context lang "en"</param>
    /// <param name="width">The max size</param>
    /// <returns>Some custom imagetag</returns>
    string GetHtml(ContentReference contentReference, string? insertingToContentReference = null, string? propname = null, string? lang = null, int? width = 0);
}
```

Remember to register your service =>  services.AddSingleton<IAITinyMceTemplateResolver, XTinyMceTemplateResolver>();

### IPlaceholderResolver

Create your custom placeholders. Within a text field, you can use patterns like ::this:: or ::pageid:5:main body:: and the text will automatically be substituted with the relevant content. The 'IPlaceholderResolver' interface allows you to define your own logic for these placeholders.

```csharp
    public interface IPlaceholderResolver
    {
        /// <summary>
        /// Lower values run first. The built-in resolvers use -120 and -110.
        /// </summary>
        int SortOrder { get; } 

        /// <summary>
        /// Will replace keywords like contentid within ::contentid:5:mainbody:: or ::this::
        /// </summary>
        /// <param name="placeholder">the complete placeholder => example "::contentid:5:mainbody::"</param>
        /// <param name="value">the content of placeholder => example "contentid:5:mainbody"</param>
        /// <param name="currentContent">The current content</param>
        /// <param name="currentCulture">The current culture</param>
        /// <param name="property">The current property</param>
        /// <param name="text">the text sent from UI</param>
        /// <param name="isHtml">If it is comming from Rich Text Editor</param>
        /// <returns>string or null if not appicable</returns>
        /// <exception cref="ArgumentException"></exception>
        string ReplacePlaceholder(string placeholder, string value, IContent currentContent, CultureInfo currentCulture, string currentProperty, string text, bool isHtml = false);
    }
```

## Example Placeholder "productcode"

Here is an example for Optimizely Commerce, if you want to use the pattern ::code:P-15254:: to point to the product with productcode "P-15254"

```csharp
using Epicweb.Optimizely.AIAssistant;
using Epicweb.Optimizely.AIAssistant.Services;
using System.Globalization;

namespace Foundation.Infrastructure
{
    public class ProductCodeAIPlaceholder: IPlaceholderResolver
    {
        
        public ProductCodeAIPlaceholder(IContentLoader contentLoader, ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
        }
        private readonly string[] keywords = { "code", "productcode" };
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;

        public int SortOrder => 100;
        /// <summary>
        /// Will replace keywords like code within ::code:P-654987:: or ::productcode:P-654987:: and all that starts with "P-" eg ::P-654987::
        /// </summary>
        /// <param name="placeholder">the complete placeholder => example "::code:P-654987::"</param>
        /// <param name="value">the content of placeholder => example "code:P-654987"</param>
        /// <param name="currentContent">The current content</param>
        /// <param name="currentCulture">The current culture</param>
        /// <param name="property">The current property</param>
        /// <param name="text">the text sent from UI</param>
        /// <param name="isHtml">If it is comming from Rich Text Editor</param>
        /// <returns>string or null if not appicable</returns>
        /// <exception cref="ArgumentException"></exception>
        public string ReplacePlaceholder(string placeholder, string value, IContent currentContent, CultureInfo currentCulture, string property, string text, bool isHtml = false)
        {
            if (value == null)
            {
                return value;
            }

            string productCode = null;
            bool startsWithKeyword = false;
            string propname = null;

            string[] parts = value.Split(':');
            if (parts.Length > 0)
            {
                value = value.Trim().ToLower();
                startsWithKeyword = keywords.Any(keyword => value.StartsWith(keyword));

                if (startsWithKeyword && parts.Length > 1)
                {
                    productCode = parts[1];
                }
                else
                    startsWithKeyword = false;

                if (value.ToUpper().StartsWith("P-"))
                {
                    startsWithKeyword = true;
                    productCode = parts[0];
                }
            }

            if (startsWithKeyword && !string.IsNullOrEmpty(productCode))
            {
                var reference = _referenceConverter.GetContentLink(productCode);
                if (_contentLoader.TryGet<IContent>(reference, currentCulture, out currentContent))
                {
                    return HelperService.GetContentFromObject(currentContent);
                }
            }

            return null;
        }
    }
}
```

## IPromptShortcut

![image](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/bc57c229-802d-45bb-a59a-f07613070a6f)

As a developer you can add Shortcut prompts, this is perfect if your organization reuse prompts on many places. The default shortcuts can be disabled. And you can disable shortcuts per property. 

```csharp
namespace Epicweb.Optimizely.AIAssistant
{
   public interface IPromptShortcut
 {
     /// <summary>
     /// Order of the Shortcut agaist other, build in has -100 if needed to override
     /// </summary>
     int SortOrder { get; }

     /// <summary>
     /// Order of the resolver agaist other, build in has -100 if needed to override
     /// </summary>
     string Name { get; }
     /// <summary>
     /// Only implement this one if it should be visible like a submenu to a shortcut
     /// </summary>
     string ParentName { get; }
     /// <summary>
     /// Enabled Visible in Input and Textarea
     /// </summary>
     bool Enabled { get; set; }
     /// <summary>
     /// Enabled and visible in RichTextEditor
     /// </summary>
     bool EnabledInRichTextEditor { get; set; }

     /// <summary>
     /// The message to send to the user if couldn't generate any prompt (maybe missing input text)
     /// </summary>
     string EmptyMessage { get; set; }

     /// <summary>
     /// When implementing this method you implement shortcuts and in this method you need to implement the logic of the prompt
     /// </summary>
     /// <param name="textContent"></param>
     /// <param name="currentContent"></param>
     /// <param name="currentCulture"></param>
     /// <param name="currentProperty"></param>
     /// <param name="isRichTextEditor">If the shortcut is used in the RTE</param>
     /// <returns>the textContent preferably starting with #</returns>
     string GeneratePrompt(string textContent, IContent currentContent, CultureInfo currentCulture, string currentProperty, bool isRichTextEditor);
}
```

remember to register your shortcut =>  services.AddSingleton<IPromptShortcut, MyPromptShortcut>();

### Disable shortcuts

Set `[AIAssistant(ShortcutsDisabled = true)]` to hide shortcuts on a property in the AI text editor.

#### Disable shortcuts in AI text editors

```json
{
  "Epicweb": {
    "AIAssistant": {
      "DisableShortcuts": true
    }
  }
}
```

#### Disable a specific shortcut 

Set the shortcut's `Enabled` and `EnabledInRichTextEditor` properties to `false` to hide it from both default lists. Remove it from any property's explicit `Shortcuts` list as well.


## Azure Open AI Provider

Create an Azure OpenAI resource and deploy a model. Use the deployment name in `AIModel`. `ServiceUrl` is the resource endpoint. `ApiUrl` is the AI Assistant service URL and can be set separately if Epicweb supplied a custom URL.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "<azure-resource-key>",
      "ServiceUrl": "https://<resource-name>.openai.azure.com/",
      "ProviderName": "AzureOpenAI",
      "AIModel": "<azure-deployment-name>"
    }
  }
}
```

### Setup Azure OpenAI service models

See [how to set up Azure OpenAI](configuration-azure-openai.md).

## Pre process the AI Request (IPreProcessAiRequest)

IPreProcessAiRequest represents an interface for pre-processing AI request data before sending it to AI

```csharp
namespace Epicweb.Optimizely.AIAssistant
{
    /// <summary>
    /// Represents an interface for pre-processing AI request data before sending it to AI.
    /// </summary>
    public interface IPreProcessAIRequest
    {
        /// <summary>
        /// Represents an interface for pre-processing AI request data before sending it to AI.
        /// </summary>
        /// <param name="modelToBeSentToAI">The AI suggestion request model to be sent to AI.</param>
        /// <param name="jsonFromFrontEnd">The request model received from the front end.</param>
        /// <returns>The modified AI suggestion request model.</returns>
        AiSuggestionRequest PreProcessData(AiSuggestionRequest modelToBeSentToAI, RequestModel jsonFromFrontEnd);
    }
}
```
