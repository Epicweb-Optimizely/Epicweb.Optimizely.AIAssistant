# Configuration

## Attributes

### UIHint

[UIHint(AIHint.Input)] => for one line input fields (string)

[UIHint(AIHint.Textarea)] => for multi line input fields (string)

[UIHint(AIHint.Image)] => for image fields (ContentReference or Url)

### [AIUseToAnalyzeContent]

You can utilize prompts with the placeholder ::this::. This means it can comprehend and integrate with the context of the page, block, product, or any other object the user is currently engaging with.

To ensure this functions correctly, you should annotate the model's property with [AIUseToAnalyzeContent]. However, it's important not to apply it to every property—only those that are relevant to the context of the object.

## TinyMCE (XHtmlString-properties)

Add "ai_assistant_execute ai_assistant_image" to Toolbar in TinyMceConfiguration

## Image Generation

Add [UIHint(AIHint.Image)] for image fields (ContentReference or Url)

Files are automatically saved in "For this page" - except for Product or other object that do not have "For this page", then it saved to the Global Root Asset folder. This default behavior can be modified either by implementing the IFolderResolver interface or by overriding the DefaultFolderResolver.

### OpenAI Api key

You need your own API key for the Image Generation to work, you can obtain a key by registering and create an key here => https://platform.openai.com/account/api-keys 

add the key to your appsettings: 
'  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-NpPD....jrwm"
      }
    }'
