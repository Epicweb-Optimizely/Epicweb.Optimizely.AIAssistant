# Configuration

## Attributes

### UIHint

**[UIHint(AIHint.Input)]** => for one line input fields (string)

**[UIHint(AIHint.Textarea)]** => for multi line input fields (string)

**[UIHint(AIHint.Image)]** => for image fields (ContentReference or Url)

### [AIUseToAnalyzeContent]

You can utilize prompts with the placeholder ::this::. This means it can comprehend and integrate with the context of the page, block, product, or any other object the user is currently engaging with.

To ensure this functions correctly, you should annotate the model's property with [AIUseToAnalyzeContent]. However, it's important not to apply it to every property—only those that are relevant to the context of the object.

### [AIAssistant]

#### For text fields
**[AIAssistant(Model = "gpt-3.5-turbo-16k")]** => The model to use => https://platform.openai.com/docs/models 

For Azure, use only the models you deployed in your azure instance.

**[AIAssistant(AssistantInstructions = "You are a selling professional sales assistant")]** => It's designed to set the behavior of the assistant. You might use the message to specify a role or provide some context for the assistant.

**[AIAssistant(ShortcutsDisabled = false)]** => Disable and hide Shortcuts on this property

**[AIAssistant(Shortcuts = new[] {
        typeof(SuggestPromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.TranslatePromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.ShortenPromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.ElaboratePromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.SummarizeShortPromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.SummarizePromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.ChangeTonePromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.HumorPromptShortcut),//you need to add all subprompts for "Change tone" if you want to use them
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.SeriousPromptShortcut),
        typeof(Epicweb.Optimizely.AIAssistant.Shortcuts.CheckSpellingPromptShortcut) })]** => Only specify the ones you want to use on this property

#### For XHtmlString fields (Rich text editor)

**[AIAssistant(QueryParameters = "")]** => Additional querystring appended to imageurl into XHtmlString (TinyMCE) eg width=100&height=300&format=webp&quality=75

**[AIAssistant(ImageWidth = "")]** => Default Imagewidth added to attribute width on img-tag into XHtmlString (TinyMCE)

**[AIAssistant(Shortcuts = new[] { ... })] => same as above, only specify the ones you want to use on this property

#### For Image fields (ContentReference or Url)

**[AIAssistant(ImageGenerationSize = ImageSize.Large)]** => ImageSize to generate Image in, Small is 256x256, Medium is 512x512, Large is 1024x1024


## TinyMCE (XHtmlString-properties)

Add "ai_assistant_execute ai_assistant_choices ai_assistant_image" to Toolbar in TinyMceConfiguration

## Image Generation

Add [UIHint(AIHint.Image)] for image fields (ContentReference or Url)

Add "ai_assistant_image" to Toolbar in TinyMceConfiguration

Files are automatically saved in "For this page" - except for Commerce products or other object that do not have "For this page", then it saved to the Global Root Asset folder. This default behavior can be modified either by implementing the IFolderResolver interface or by overriding the DefaultFolderResolver.

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

## Overide default behaviors

You can override these implementations if you want to implement different logic

### IAssistantInstructionsResolver

This allows for the implementation of global 'Assistant Instructions'.

The default setup dictates that if the Startpage contains a field named 'AssistantInstructions', that field's content will be sent to AI ChatGPT. This approach is recommended, as it enables site editors to easily alter and optimize the instruction.

```
public interface IAssistantInstructionsResolver
{
    string GetInstructions();
}
```

Remember to register your implementation => services.AddSingleton<IAssistantInstructionsResolver, ...>();

```
public string GetInstructions()
{
    ContentReference start = ContentReference.StartPage;
    if (contentLoader.TryGet(start, out PageData startpage) && startpage.Property.TryGetPropertyValue<string>("AssistantInstructions", out string value)) {
        return value;
    }
    return null;
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

Version 1.2 has the posibility to use Azure OpenAI Services. To use the service you need to set up your own service in your Azure instance. For the moment Azure Models are available in Australia East, Canada East, East US, East US 2, France Central, Japan East, North Central US, Sweden Central, Switzerland North, UK South. Read up to date information here: https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models#gpt-35-models

```
  "Epicweb": {
    "AIAssistant": {
      "ApiUrl": "", //azure service url provided by Epicweb upon registration of add-on
      "ApiKeyImage": "sk-NpPD*****", //open ai key for Images (if used)
      "ApiKey": "06de6816f*****", //azure key
      "ServiceUrl": "https://[changethis].openai.azure.com/",
      "ProviderName": "AzureOpenAI",
      "AIModel": "gpt-35-turbo-16k",//the default model if nothing else is specified on property
      }
    }
```


