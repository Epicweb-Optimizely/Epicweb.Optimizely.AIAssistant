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

For Azure, use only the models you deployed in your azure instance.

**[AIAssistant(AssistantInstructions = "You are a selling professional sales assistant")]** => It's designed to set the behavior of the assistant. You might use the message to specify a role or provide some context for the assistant.

**[AIAssistant(MinimalMode = true)]** => Default is false, hides the "AI Wheel" by default

**[AIAssistant(AutoSuggest = false)]** => Default is true, When you leave the field, it will execute the AI to AutoSuggest an alternative suggestion

**[AIAssistant(ShortcutsDisabled = false)]** => Disable and hide Shortcuts on this property

**[AIAssistant(CustomJson = "{\"prop\":\"fromAttribute\"}")]** => Any custom data you want to send to your API, to be used with CustomAI Provider

```
 "Epicweb": {
    "AIAssistant": {      
      "CustomJson": "{\"prop\":\"fromAppsettings\"}",// Any custom data you want to send to your API
      }
    }
```

**[AIAssistant(Roles = new[] { "AIEditors", "SomeRole" })** User roles can be assigned globally or on a per-property basis to manage access to AI-Assistant

```
 "Epicweb": {
    "AIAssistant": {
      "Roles": [ "AIEditors", "SomeRole" ] //sets globally
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
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.CheckSpellingPromptShortcut) })]** => Only specify the ones you want to use on this property

**Additional shortcut of value: **

SeoTitlePromptShortcut (for creation of SEO titles based on page content) 

SEOKeywordsPromptShortcut (for creation of keywords based on page content)

you need to add them to services => 
```
services.AddSingleton<IPromptShortcut, SEOKeywordsPromptShortcut>();
services.AddSingleton<IPromptShortcut, SeoTitlePromptShortcut>();
```

#### For XHtmlString fields (Rich text editor)

**[AIAssistant(QueryParameters = "")]** => Additional querystring appended to imageurl into XHtmlString (TinyMCE) eg width=100&height=300&format=webp&quality=75

**[AIAssistant(ImageWidth = "")]** => Default Imagewidth added to attribute width on img-tag into XHtmlString (TinyMCE)

**[AIAssistant(Shortcuts = new[] { ... })]** => same as above, only specify the ones you want to use on this property

**[AIAssistant(ImageGenerationSize = "1024x1024")]** =>  ImageSize to generate Image in, dall-e-3: 1024x1024, 1024x1792 or 1792x1024 - RecraftAI: 1024x1024, 1365x1024, 1024x1365, 1536x1024, 1024x1536, 1820x1024, 1024x1820, 1024x2048, 2048x1024, 1434x1024, 1024x1434, 1024x1280, 1280x1024, 1024x1707, 1707x1024

**[AIAssistant(ImageGenerationStyle = "vivid")]** => The style of the generated images. Must be one of vivid or natural for Dalle-3. Vivid causes the model to lean towards generating hyper-real and dramatic images. Natural causes the model to produce more natural, less hyper-real looking images. This param is only supported for dall-e-3. Defaults to vivid.
For Recraft AI: https://www.recraft.ai/docs#list-of-styles create a querystring "style=digital_illustration|pixel_art", default is realistic_image  [Link to config](recraftai-image-generation.md)
    

**[AIAssistant(ImageModel = "dall-e-3")]** => The Dall-e model to use => https://platform.openai.com/docs/models/dall-e (dall-e-3 or recraft), default to dall-e-3 on latest version

#### For Image fields (ContentReference or Url)

**[AIAssistant(ImageGenerationSize = "1024x1024")]** =>  ImageSize to generate Image in, dall-e-3: 1024x1024, 1024x1792 or 1792x1024 - RecraftAI: 1024x1024, 1365x1024, 1024x1365, 1536x1024, 1024x1536, 1820x1024, 1024x1820, 1024x2048, 2048x1024, 1434x1024, 1024x1434, 1024x1280, 1280x1024, 1024x1707, 1707x1024
   
**[AIAssistant(ImageGenerationStyle = "vivid")]** => The style of the generated images. Must be one of vivid or natural for Dalle-3. Vivid causes the model to lean towards generating hyper-real and dramatic images. Natural causes the model to produce more natural, less hyper-real looking images. This param is only supported for dall-e-3. Defaults to vivid.
For Recraft AI: https://www.recraft.ai/docs#list-of-styles create a querystring "style=digital_illustration|pixel_art", default is realistic_image  

**[AIAssistant(ImageModel = "dall-e-3")]** => The Dall-e model to use => https://platform.openai.com/docs/models/dall-e (dall-e-3 or recraft), default to dall-e-3 on latest version

## Appsettings

### Disable AutoSuggest behavior

**[AIAssistant(AutoSuggest = false)]** => Disable Auto Suggest on leavning the field

#### Disable Auto Suggest globaly => add in appsetting:  

```
 "Epicweb": {
    "AIAssistant": {
      "AutoSuggest": false //sets globally, default true
      }
    }
```
### Enable Minimal Mode

Field is minimal - hides the "AI Wheel" by default

**[AIAssistant(MinimalMode = true)]** => Default is false, 

#### Enable MinimalMode globally => add in appsetting:  

```
 "Epicweb": {
    "AIAssistant": {
      "MinimalMode": true //sets globally, default false
      }
    }
```

## TinyMCE Rich Text Editor (XHtmlString-properties)

Add "ai_assistant_execute ai_assistant_choices ai_assistant_image" to Toolbar in TinyMceConfiguration

You can remove the AI integration from the RTE by using services.AddAIAssistant(addToDefaultTinyMCE: false).

To add AI to a custom configuration of blocks and pages, use ((TinyMceSettings)config).AddAIAssistantToTinyMCE().

## Image Generation

Add [UIHint(AIHint.Image)] for image fields (ContentReference or Url)

Add "ai_assistant_image" to Toolbar in TinyMceConfiguration

Files are automatically saved in "For this page" - except for Commerce products or other object that do not have "For this page", then it saved to the Global Root Asset folder. This default behavior can be modified either by implementing the IFolderResolver interface or by overriding the DefaultFolderResolver.

### Modifying File Names and Image Descriptions

By implementing your own IMediaPropertyResolver, you can modify media data through post-processing. This includes updating the Description property, setting the Name property, and adjusting the image size as necessary.

### OpenAI Api key

You need your own API key for the Image Generation to work, you can obtain a key by registering and create an key here => https://platform.openai.com/account/api-keys 

add the key to your appsettings: 
```
 "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-NpPD....jrwm"
      }
    }
```

### Image Properties

If you need to change default behavior, these fields kan be override by AIAssistantAttribute on each property 
```
 "Epicweb": {
    "AIAssistant": {
        "ImageModel": "dall-e-3", // or dall-e-2
        "ImageStyle": null // vivid or natural, Defaults to vivid
      }
    }
```

[Recraft Image Generation Configuration](recraftai-image-generation.md)

## Override default behaviors

You can override these implementations if you want to implement different logic

### IAssistantInstructionsResolver

This allows for the implementation of global 'Assistant Instructions'.

The default setup dictates that if the Startpage contains a field named 'AssistantInstructions', that field's content will be sent to AI. This approach is recommended, as it enables site editors to easily alter and optimize the instruction.

When working with translations, we have a dedicated API called "GetInstructionsForTranslations." This API allows you to provide translation instructions by language. You can also include information on how to translate, along with a list of terms or translation rules. 

```
public interface IAssistantInstructionsResolver
{
    string GetInstructions();
    string GetInstructions(CultureInfo language);
    string GetInstructionsForTranslations(CultureInfo toLanguage, CultureInfo? fromLanguage = null);
}
```

Remember to register your implementation => services.AddSingleton<IAssistantInstructionsResolver, ...>();

```
 public string GetInstructions()
{
    return this.GetInstructions(null);
}

public string GetInstructions(CultureInfo language)
{
    ContentReference start = ContentReference.StartPage;
    if (contentLoader.TryGet(start, out PageData startpage) && startpage.Property.TryGetPropertyValue("AssistantInstructions", out string value))
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
    return configuration["Epicweb:AIAssistant:AssistantInstructions"];
}
```
### IAITinyMceTemplateResolver

Override the IAITinyMceTemplateResolver to implement your own image-tag implementation in the rich text editor

```
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

```
    public interface IPlaceholderResolver
    {
        /// <summary>
        /// Order of the resolver agaist other, build in has -100 if needed to override
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

```
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
                    return AIHelperService.GetContentFromObject(currentContent);
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

```
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

**[AIAssistant(ShortcutsDisabled = false)]** => Disable and hide Shortcuts on a property

#### Disable shortcuts globaly => add in appsetting:  

```
 "Epicweb": {
    "AIAssistant": {
      "DisableShortcuts": false
      }
    }
```

#### Disable a specific shortcut 

ServiceLocator.Current.GetAllInstances<IPromptShortcut>().FirstOrDefault(x => x.Name == SalesPromptShortcut.GetName)?.Enabled = false;


## Azure Open AI Provider

Native out of the box support of Azure OpenAI Services. In order to use this service, you must establish your own service within your Azure instance. Currently, Azure Models are accessible in the following regions: Australia East, Canada East, East US, East US 2, France Central, Japan East, North Central US, Sweden Central, Switzerland North, and UK South. For the most recent information, please refer to the following resource: https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models

```
  "Epicweb": {
    "AIAssistant": {
      "ApiUrl": "", //azure service url provided by Epicweb upon registration of add-on
      "ApiKeyImage": "sk-NpPD*****", //open ai key for Images (if used)
      "ApiKey": "06de6816f*****", //azure key
      "ServiceUrl": "https://[changethis].openai.azure.com/",
      "ProviderName": "AzureOpenAI",
      "AIModel": "gpt-4o-mini",//the default model if nothing else is specified on property
      }
    }
```

### Setup Azure OpenAI service models

Read more here https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/blob/master/configuration-azure-openai.md

## Pre process the AI Request (IPreProcessAiRequest)

IPreProcessAiRequest represents an interface for pre-processing AI request data before sending it to AI

```
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
