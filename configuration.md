# Configuration

## Attributes

### UIHint

[UIHint(AIHint.Input)] => for one line input fields (string)
[UIHint(AIHint.Textarea)] => for multi line input fields (string)
[UIHint(AIHint.Image)] => for image fields (ContentReference or Url)

### [AIUseToAnalyzeContent]

todo

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
