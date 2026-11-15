# Working with translations

The translations capabilities with the AI Assistant are highly configurable and support both manual field-by-field translations and automated bulk translations using the LanguageManager integration.

## Overview

### For Content Editors

The AI Assistant provides two translation workflows:

1. **Manual Translation** - Translate individual properties one at a time within the content editor using inline AI assistance
2. **Bulk Translation** - Translate entire pages or blocks using the LanguageManager plugin, which can translate multiple properties simultaneously across content hierarchies

### For Developers

The solution offers:

- Flexible configuration of translation behavior
- Integration with LanguageManager for automated translation workflows
- Configurable exclusion and preservation rules for specific content types and properties
- Per-language URL segment handling strategies
- Custom instruction support via IAssistantInstructionsResolver

---

## Features

### Manual Field Translation
- Translate one field at a time using the AI Property Wrapper
- Inline AI suggestions with support for multiple languages
- Real-time translation within the content editor

### Bulk Translation with LanguageManager
- Translate entire pages or block hierarchies at once
- Configurable content type and property exclusions
- Automatic URL segment generation strategies per language
- Skip existing translations by default with optional force-retranslation
- Recursive nested content translation support
- Preserve specific properties from being translated (e.g., identifiers, technical fields, block names)

---

## Installation

### Step 1: Install NuGet Packages

Install the latest versions:

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
services.AddLanguageManager();
services.AddLanguageManagerAIAssistant();
services.AddLanguageManagerOverrides(Configuration);  // Optional: for advanced configuration
```

### Step 3: Configure AI Provider (Editor UI)

1. Log in to Optimizely CMS Admin
2. Go to **Settings** → **Language Manager**
3. Select **AI-Assistant** as the translation provider
4. No API key or region configuration needed (uses your existing AI Assistant settings)

---

## Configuration & Customization

### Basic Instructions

All translation instructions are defined through the IAssistantInstructionsResolver interface, which allows you to:

- Set global translation instructions
- Override instructions per language
- Provide language-specific terminology and translation rules

#### Implement IAssistantInstructionsResolver

```csharp
public interface IAssistantInstructionsResolver
{
    string GetInstructions();
    string GetInstructions(CultureInfo language);
    string GetInstructionsForTranslations(CultureInfo toLanguage, CultureInfo? fromLanguage = null);
}
```

Register your implementation:

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

    public string GetInstructions()
    {
        return GetInstructions(null);
    }

    public string GetInstructions(CultureInfo language)
    {
        ContentReference start = ContentReference.StartPage;
        if (_contentLoader.TryGet(start, out PageData startpage) && 
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

The LanguageManager Overrides system allows fine-grained control over which content and properties are translated. This is configured through appsettings.json.

#### Configuration Structure

Add the LanguageManagerOverrides section to your appsettings.json:

```json

  "LanguageManagerOverrides": {
    // When true: base configured type excludes derived types too. When false: only exact type match.
    "ApplyToDerivedContentTypes": true,
    // Content types that should NOT be translated - values are copied/preserved from source instead
    "ExcludedContentTypes": [
      // EPiServer Forms blocks are excluded from translation, block reference is preserved
      "EPiServer.Forms.Implementation.Elements.FormContainerBlock",
      // Alloy TextBlock is excluded from translation, block reference is preserved
      "AlloySandbox.Models.Blocks.TextBlock"
    ],
    // Properties that should not be translated
    "ExcludedProperties": {
      // Global: Property names excluded on all content types (supports Provider.PropertyName format)
      "Global": [
        // "PageURLSegment", // Uncomment to globally prevent URL segment changes
        // CatalogContent.SeoUri is excluded on all content types
        "CatalogContent.SeoUri",
        // Technical identifier field is never translated
        "TechnicalIdentifier"
      ],
      // Per-type property exclusions: Properties excluded only on specific content types
      "ByContentType": {
        // ArticlePage type-specific excluded properties (if this content type exists in your project)
        "MyProject.Models.Pages.ArticlePage": [
          // ExternalUrl property is excluded from translation for ArticlePages
          "ExternalUrl",
          // CustomMetadata property is excluded from translation for ArticlePages
          "CustomMetadata"
        ],
        "AlloySandbox.Models.Blocks.TestBlock": [
          // Copyright property should not be copied from source for TestBlocks, it is excluded from translation
          "Copyright"
        ],
        // TeaserBlock type-specific excluded properties (if this content type exists in your project)
        "MyProject.Models.Blocks.TeaserBlock": [
          // TrackingCode property is excluded from translation for TeaserBlocks
          "TrackingCode"
        ]
      }
    },
    // Properties that should always be copied from source (never translated)
    "PreserveSourceProperties": {
      // Global
      "Global": [ "PageName" ],
      // Per-type preserve-source properties: Matching properties are copied from source during translation
      "ByContentType": {
        "AlloySandbox.Models.Blocks.TestBlock": [
          // Copyright property is preserved from source without translation
          "Copyright"
        ],
        // "EPiServer.Core.PageData": [ "PageName" ], // Uncomment to preserve page names from source
        // BlockData base type: Name property is preserved/copied from source without translation
        // Note: With ApplyToDerivedContentTypes=true, this applies to all derived BlockData types
        "EPiServer.Core.BlockData": [ "Name" ]
        // TestBlock type-specific: Copyright property is preserved from source
        //"AlloySandbox.Models.Blocks.TestBlock": [
        //  // Copyright property is preserved from source without translation
        //  "Copyright"
        //]
      }
    },
    // Translation behavior controls
    "Translation": {
      // When true: skip translation if target branch already contains meaningful values (default behavior)
      "SkipExistingTranslations": true,
      // When true: forces update of existing translations and overrides skip behavior
      "ForceRetranslation": false
    },
    // URL segment behavior per language
    "UrlSegments": {
      // Fallback mode for languages not explicitly mapped (Default, PreserveSource, or FromName)
      // Default: translate URL segment with BuildAsciiSlug()
      // PreserveSource: copy source PageURLSegment without modification
      // FromName: generate ASCII-safe slug from translated page title (PageName)
      "DefaultMode": "Default",
      // Per-language URL segment mode override
      "Languages": {
        // Chinese (simplified): preserve URL segment from source (Page/Block Name still translates unless configured separately)
        "zh": "PreserveSource",
        // French: preserve URL segment from source (Page/Block Name still translates unless configured separately)
        // Important: To also preserve Name/PageName, configure it separately in PreserveSourceProperties
        "fr": "PreserveSource",
        // Swedish: generates URL segment from translated Title (uncomment if Title field exists and is translated)
        // "sv": "FromName",
        // Japanese: use default mode (translate URL segment with BuildAsciiSlug)
        "ja": "Default"
      }
    }
  }
```

#### Configuration Options Reference

| Setting | Type | Description |
|---------|------|-------------|
| **ApplyToDerivedContentTypes** | boolean | When true, exclusion rules for base types apply to all derived types (e.g., rules for BlockData apply to all blocks). Default: true |
| **ExcludedContentTypes** | string[] | Content type names (full CLR names) that should never be translated. AI translation is skipped; values are copied from source. |
| **ExcludedProperties.Global** | string[] | Properties excluded from translation on ALL content types. Supports provider-qualified format (Provider.PropertyName) and plain property names. |
| **ExcludedProperties.ByContentType** | object | Per-content-type property exclusions. Maps content type names to arrays of property names. |
| **PreserveSourceProperties.Global** | string[] | Properties that should always be copied directly from source language without translation, on all content types. |
| **PreserveSourceProperties.ByContentType** | object | Per-content-type properties that preserve source values. Maps content type names to arrays of property names. |
| **Translation.SkipExistingTranslations** | boolean | When true, skips translation if the target language branch already contains meaningful values. Default: true |
| **Translation.ForceRetranslation** | boolean | When true, forces re-translation of existing branches. Overrides skip behavior. Default: false |
| **UrlSegments.DefaultMode** | string | Default URL segment generation strategy for languages not explicitly configured. Options: Default, PreserveSource, FromName. Default: Default |
| **UrlSegments.Languages** | object | Per-language URL segment strategies. Maps language codes to strategy names. |

#### URL Segment Strategies

The URL segment (page slug) can be handled differently per language:

- **Default**: Translates the URL segment along with other content using AI, then applies ASCII-safe slug formatting
- **PreserveSource**: Copies the source language URL segment without modification (useful for languages like Chinese or Japanese where translated slugs may not be meaningful)
- **FromName**: Generates URL segment from the translated page/block Name/PageName, ensuring title-based, language-appropriate slugs

> **Important**: Preserving the URL segment does NOT automatically preserve the page/block Name. If you want both to remain unchanged, configure the Name separately in PreserveSourceProperties.

#### Common Configuration Scenarios

**Scenario 1: Exclude form blocks and technical properties globally**

```json
{
  "LanguageManagerOverrides": {
    "ExcludedContentTypes": [
      "EPiServer.Forms.Implementation.Elements.FormContainerBlock"
    ],
    "ExcludedProperties": {
      "Global": [
        "TechnicalIdentifier",
        "TrackingCode",
        "CustomMetadata"
      ]
    }
  }
}
```

**Scenario 2: Preserve block names and handle Chinese URLs**

```json
{
  "LanguageManagerOverrides": {
    "PreserveSourceProperties": {
      "ByContentType": {
        "EPiServer.Core.BlockData": [ "Name" ]
      }
    },
    "UrlSegments": {
      // Fallback mode for languages not explicitly mapped (Default, PreserveSource, or FromName)
      // Default: translate URL segment with BuildAsciiSlug()
      // PreserveSource: copy source PageURLSegment without modification
      // FromName: generate ASCII-safe slug from translated page title (PageName)
      "DefaultMode": "Default",
      "Languages": {
        "zh": "PreserveSource",
        "zh-CN": "PreserveSource",
        "zh-TW": "PreserveSource",
        "sv": "FromName"
      }
    }
  }
}
```

**Scenario 3: Exclude specific properties per content type**

```json
{
  "LanguageManagerOverrides": {
    "ExcludedProperties": {
      "ByContentType": {
        "MyProject.Models.Pages.ArticlePage": [
          "MyProperty",
          "OtherProperty"
        ],
        "MyProject.Models.Pages.ProductPage": [
          "SKU",
          "Price"
        ]
      }
    }
  }
}
```

**Scenario 4: Force retranslation of all branches**

```json
{
  "LanguageManagerOverrides": {
    "Translation": {
      "SkipExistingTranslations": false,
      "ForceRetranslation": true
    }
  }
}
```

---

## Using the Translation API

For programmatic translation (e.g., in scheduled tasks or custom workflows), use the TranslationService:

```csharp
public class TranslationService
{
    public Task<string> TranslateText(
        string input, 
        CultureInfo? fromLanguage, 
        CultureInfo toLanguage, 
        string source = null)
    {
        // Translates text from source language to target language
    }
}
```

Inject and use it in your custom code:

```csharp
[Inject]
public TranslationService TranslationService { get; set; }

public async Task TranslateMyContent()
{
    string translated = await TranslationService.TranslateText(
        "Hej världen",           // input text
        new CultureInfo("sv"),   // from Swedish
        new CultureInfo("en"),   // to English
        "My content page"        // source context
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
5. All content and nested blocks are translated according to your configuration

[Video: Language Manager Bulk Translation](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/aea7c323-47f7-45c1-93a9-9185e5074e0f)





