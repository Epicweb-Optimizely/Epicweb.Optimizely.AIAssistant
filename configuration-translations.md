 # Working with translations

 The translations capabilities with the AI Assistant are highly configurable

 ## Features

 - translate one field at time
 - translate all fields on a page/block type with the [LanguageManager plugin](Translations-with-LanguageManager.md)

 ## Implement IAssistantInstructionsResolver

This allows for the implementation of global 'Assistant Instructions' but also different instruction between languages if needed. 

When working with translations, we have a dedicated API called "GetInstructionsForTranslations." This API allows you to provide translation instructions by language. You can also include information on how to translate, along with a list of terms or translation rules. 

```csharp
public interface IAssistantInstructionsResolver
{
    string GetInstructions();
    string GetInstructions(CultureInfo language);
    string GetInstructionsForTranslations(CultureInfo toLanguage, CultureInfo? fromLanguage = null);
}
```

Remember to register your implementation => services.AddSingleton<IAssistantInstructionsResolver, ...>();

```csharp
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
    return configuration["Epicweb:AIAssistant:AssistantTranslationInstructions"];
}
```

## Examples

### **Scenario 1:** English page exists with text in the property "page description", change to Swedish context of the page, spinn the wheel and it translates the field.

| ![Epicweb Optimizely AIAssistant_Scenario_Translations1](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/5973da64-f645-43f0-93eb-6769bb1170fb) |
|-|

<br /><br />

### **Scenario 2:** Go to Swedish context of the page, paste in the English text, spinn the wheel and it translates the field to Swedish automatically.

| ![Epicweb Optimizely AIAssistant_Scenario_Translations2](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/110e46a3-0538-462e-adde-ea0e47ca0d98) |
|-|

<br /><br />

### **Scenario 3:** Translate text to any language

| ![Epicweb Optimizely AIAssistant_Scenario_Translations3](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/96da3776-8fde-4e4e-b81a-cb076d6ea8ae) |
|-|

<br /><br />

### **Scenario 4:** Language Manager

https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/aea7c323-47f7-45c1-93a9-9185e5074e0f
