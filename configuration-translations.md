# Working with translations

The translations capabilities with the AI Assistant are highly configurable and support both manual field-by-field translations and automated bulk translations using the LanguageManager integration.

## Overview

### For Content Editors

The AI Assistant provides two translation workflows:

1. **Manual Translation** - Translate individual properties one at a time within the content editor using inline AI assistance
2. **Bulk Translation** - Translate entire pages or blocks using the LanguageManager plugin, which can translate multiple properties simultaneously across content hierarchies

### For Developers

The AI Assistant translation provider integrates with LanguageManager and supports custom instructions via `IAssistantInstructionsResolver`. A project that adds LanguageManager Overrides can also configure content exclusions, existing-branch handling, and URL segments.

---

## Features

### Manual Field Translation
- Translate one field at a time using the AI Property Wrapper
- Inline AI suggestions with support for multiple languages
- Real-time translation within the content editor

### Bulk Translation with LanguageManager
- Translate entire pages or block hierarchies at once
- LanguageManager handles language branches and nested content using its configured branch manager

---

## Installation

### Step 1: Install NuGet Packages

Install versions compatible with your CMS and .NET target. See the [LanguageManager package requirements](translations-with-LanguageManager.md).

```
Install-Package EPiServer.Labs.LanguageManager
Install-Package Epicweb.Optimizely.AIAssistant.LanguageManager
```

Or via Package Manager:
- https://nuget.optimizely.com/packages/EPiServer.Labs.LanguageManager/ 
- https://nuget.optimizely.com/packages/epicweb.optimizely.aiassistant.languagemanager/ 

### Step 2: Configure Startup

Add to Program.cs or Startup.cs:

```csharp
using Epicweb.Optimizely.AIAssistant.LanguageManager;

// CMS 13 projects also register LanguageManager with services.AddLanguageManager().
services.AddLanguageManagerAIAssistant();
```

### Step 3: Configure AI Provider (Editor UI)

1. Log in to Optimizely CMS Admin
2. Go to **Settings** → **Language Manager**
3. Select **AI-Assistant** as the translation provider
4. No API key or region configuration needed (uses your existing AI Assistant settings)

---

## Configuration & Customization

### Basic Instructions

The translation service uses `IAssistantInstructionsResolver.GetInstructionsForTranslations`. With the default resolver, add `Instructions/Translation/General.md` to your registered embedded instructions, or use a language-specific file such as `General.sv.md`. See [configuration](configuration.md#iassistantinstructionsresolver).

A custom resolver can:

- Set global translation instructions
- Override instructions per language
- Provide language-specific terminology and translation rules

#### Implement IAssistantInstructionsResolver

```csharp
public interface IAssistantInstructionsResolver
{
    string GetInstructions(CultureInfo language);
    string GetInstructionsForTranslations(CultureInfo toLanguage, CultureInfo? fromLanguage = null);
}
```

Register your implementation after `AddAIAssistant()`.

```csharp
services.AddSingleton<IAssistantInstructionsResolver, YourCustomResolver>();
```

#### Example Implementation

```csharp
public class CustomInstructionsResolver : IAssistantInstructionsResolver
{
    private readonly IContentLoader _contentLoader;
    private readonly IConfiguration _configuration;

    public CustomInstructionsResolver(IContentLoader contentLoader, IConfiguration configuration)
    {
        _contentLoader = contentLoader;
        _configuration = configuration;
    }

    public string GetInstructions(CultureInfo language)
    {
        ContentReference start = ContentReference.StartPage;
        if (_contentLoader.TryGet(start, language, out PageData startpage) &&
            startpage.Property.TryGetPropertyValue("AssistantInstructions", out string value))
        {
            return value;
        }
        return _configuration["Epicweb:AIAssistant:AssistantInstructions"];
    }

    public string GetInstructionsForTranslations(CultureInfo toLanguage, CultureInfo? fromLanguage = null)
    {
        ContentReference start = ContentReference.StartPage;
        if (_contentLoader.TryGet(start, toLanguage, out PageData startpage) &&
            startpage.Property.TryGetPropertyValue("AssistantInstructions", out string value))
        {
            return value;
        }
        return _configuration["Epicweb:AIAssistant:AssistantTranslationInstructions"];
    }
}
```

### Advanced Configuration: LanguageManager Overrides

These settings apply to projects with a custom LanguageManager branch manager that supplies `AddLanguageManagerOverrides`. Check that the override extension and its options exist in your LanguageManager build before adding this setup.

```csharp
using EPiServer.Labs.LanguageManager;

services.AddLanguageManagerOverrides(Configuration);
```

The override implementation binds the `LanguageManagerOverrides` section in `appsettings.json`.

```json
{
  "LanguageManagerOverrides": {
    "ApplyToDerivedContentTypes": true,
    "ExcludedContentTypes": [
      "EPiServer.Forms.Implementation.Elements.FormContainerBlock"
    ],
    "ExcludedProperties": {
      "Global": ["TechnicalIdentifier", "CatalogContent.SeoUri"],
      "ByContentType": {
        "MyProject.Models.Pages.ArticlePage": ["ExternalUrl", "CustomMetadata"]
      }
    },
    "Translation": {
      "SkipExistingTranslations": true,
      "ForceRetranslation": false
    },
    "UrlSegments": {
      "DefaultMode": "Default",
      "Languages": {
        "zh": "PreserveSource",
        "sv": "AsciiSlug"
      }
    }
  }
}
```

| Setting | Behavior |
| --- | --- |
| `ApplyToDerivedContentTypes` | Applies rules for resolved base types to derived types. Defaults to `true`. |
| `ExcludedContentTypes` | Skips AI translation for matching content types. Use fully qualified CLR type names. |
| `ExcludedProperties.Global` and `ExcludedProperties.ByContentType` | Skips excluded properties during translation and copying. Accepts property names or provider-qualified names such as `CatalogContent.SeoUri`. `PageName` is handled separately and keeps the source name when excluded. |
| `Translation.SkipExistingTranslations` | Skips target branches that already contain meaningful values. Defaults to `true`. |
| `Translation.ForceRetranslation` | Forces translation of existing branches and takes precedence over the skip option. Defaults to `false`. |
| `UrlSegments.DefaultMode` and `UrlSegments.Languages` | Selects the URL segment behavior. Exact language codes take precedence, followed by the two-letter language code and then the default mode. |

The checked override implementation supports these modes for `PageURLSegment`.

- `Default` translates the source URL segment and formats it using the CMS URL rules.
- `PreserveSource` copies the source segment as is. Use a separate `PageName` exclusion if you also want to keep the source page name.
- `AsciiSlug` builds an ASCII slug from the source segment or, when empty, the source content name.

---

## Using the Translation API

For programmatic translation in scheduled tasks or custom workflows, inject `Epicweb.Optimizely.AIAssistant.Services.TranslationService`. Call `TranslateText` with the input, source culture, target culture, and an optional source label.

```csharp
[Inject]
public TranslationService TranslationService { get; set; }

public async Task TranslateMyContent()
{
    string translated = await TranslationService.TranslateText(
        "Hej världen",           // input text
        new CultureInfo("sv"),   // from Swedish
        new CultureInfo("en"),   // to English
        "My content page"        // source label for tracing the request
    );
}
```

---

## Translation Examples

### Scenario 1: Manual Translation - Using the Wheel Icon

English page exists with text in the "page description" property. To translate to Swedish:

1. Navigate to the page's Swedish language context
2. Click the AI wheel icon on the "page description" property
3. The AI translates and fills the field automatically

![Scenario 1](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/5973da64-f645-43f0-93eb-6769bb1170fb)

### Scenario 2: Manual Translation - Pasting English Text

When working in Swedish context with English source text:

1. Paste or have English text in the property (on Swedish page)
2. Click the AI wheel icon
3. The AI detects the source language and translates to Swedish

![Scenario 2](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/110e46a3-0538-462e-adde-ea0e47ca0d98)

### Scenario 3: Translate to Any Language

Translate content to multiple languages regardless of current page context:

1. Select text in a property
2. Click the AI wheel icon
3. Choose target language from the dialog
4. AI translates to the specified language

![Scenario 3](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/96da3776-8fde-4e4e-b81a-cb076d6ea8ae)

### Scenario 4: Bulk Translation with LanguageManager

Translate entire pages and their blocks with a single action:

1. Navigate to Language Manager module
2. Select source language and target language
3. Select content to translate
4. Choose "Translate and Copy" action
5. Review the new language version and any translated nested content

[Video: Language Manager Bulk Translation](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/aea7c323-47f7-45c1-93a9-9185e5074e0f)





