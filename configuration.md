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

**[AIAssistant(AssistantInstructions = "You are a selling professional sales assistant")]** => It's designed to set the behavior of the assistant. You might use the message to specify a role or provide some context for the assistant.

**[AIAssistant(ShortcutsDisabled = false)]** => Disable and hide Shortcuts on this property

#### For XHtmlString fields (Rich text editor)

**[AIAssistant(QueryParameters = "")]** => Additional querystring appended to imageurl into XHtmlString (TinyMCE) eg width=100&height=300&format=webp&quality=75

**[AIAssistant(ImageWidth = "")]** => Default Imagewidth added to attribute width on img-tag into XHtmlString (TinyMCE)

#### For Image fields (ContentReference or Url)

**[AIAssistant(ImageGenerationSize = "")]** => ImageSize to generate Image in, Small is 256x256, Medium is 512x512, Large is 1024x1024


## TinyMCE (XHtmlString-properties)

Add "ai_assistant_execute ai_assistant_image" to Toolbar in TinyMceConfiguration

## Image Generation

Add [UIHint(AIHint.Image)] for image fields (ContentReference or Url)

Add "ai_assistant_image" to Toolbar in TinyMceConfiguration

Files are automatically saved in "For this page" - except for Commerce products or other object that do not have "For this page", then it saved to the Global Root Asset folder. This default behavior can be modified either by implementing the IFolderResolver interface or by overriding the DefaultFolderResolver.

### OpenAI Api key

You need your own API key for the Image Generation to work, you can obtain a key by registering and create an key here => https://platform.openai.com/account/api-keys 

add the key to your appsettings: 
'  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-NpPD....jrwm"
      }
    }'

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
        /// <returns>string or null if not appicable</returns>
        /// <exception cref="ArgumentException"></exception>
        string ReplacePlaceholder(string placeholder, string value, IContent currentContent, CultureInfo currentCulture, string currentProperty, string text);
    }
```

## Example Placeholder "productcode"

Here is an example for Optimizely Commerce, you want to use the pattern ::code:P-15254:: to point to the product with productcode "P-15254"

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
        /// Will replace keywords like code within ::code:P-654987:: or ::productcode:P-654987::
        /// </summary>
        /// <param name="placeholder">the complete placeholder => example "::code:P-654987::"</param>
        /// <param name="value">the content of placeholder => example "code:P-654987"</param>
        /// <param name="currentContent">The current content</param>
        /// <param name="currentCulture">The current culture</param>
        /// <param name="property">The current property</param>
        /// <param name="text">the text sent from UI</param>
        /// <returns>string or null if not appicable</returns>
        /// <exception cref="ArgumentException"></exception>
        public string ReplacePlaceholder(string placeholder, string value, IContent currentContent, CultureInfo currentCulture, string property, string text)
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

