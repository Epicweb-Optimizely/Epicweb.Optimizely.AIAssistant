# Configuration

## Attributes

### UIHint

[UIHint(AIHint.Input)] => for one line input fields (string)

[UIHint(AIHint.Textarea)] => for multi line input fields (string)

[UIHint(AIHint.Image)] => for image fields (ContentReference or Url)

### [AIUseToAnalyzeContent]

You can use prompts with the placeholder ::this:: which means the it understands and integrate with the context of the page, block, product, or any object the user is currently interacting with.

For this to work properly, you need to decorate the models property with [AIUseToAnalyzeContent], but do not add it to every property, only the once that makes since for the context of the object. 

## Image Generation

todo

### OpenAI Api key

You need your own API key for the Image Generation to work, you can obtain a key by registering and create an key here => https://platform.openai.com/account/api-keys 

add the key to your appsettings: 
'  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-NpPD....jrwm"
      }
    }'
