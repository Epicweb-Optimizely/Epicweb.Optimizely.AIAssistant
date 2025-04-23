# Image Analyzer Configuration

By following this configuration guide, you can seamlessly integrate and customize the **Image Analyzer** functionality in your Optimizely CMS project, enabling powerful, AI-driven media management.

## Register the Image Analyzer

To enable the Image Analyzer functionality, add the following line to your application configuration:

```csharp
app.AddImageAnalyzer();
```

This ensures that the image analysis process is triggered when creating or importing content in Optimizely CMS. The analysis automatically populates media object properties based on predefined attributes.

---

### Add vision model to appsettings

```json
"Epicweb": {
    "AIAssistant": {
       "AIVisionModel": "gpt-4o"
     }
}
```
This is required to make the Analyzer to work.

## Example Prompts
The following are examples of prompts that can be used with `AnalyzeImageAttribute`:

1. **Alt Text in Multiple Languages**:
   ```csharp
    [AnalyzeImageAltText]
    public virtual IList<LocalizedString> AltTextList { get; set; }
   ```

![image](https://github.com/user-attachments/assets/b7aeac9f-f05d-4cb8-9a36-7ff156148531)


2. **Alt Text in one specific language**:
   ```csharp
   [AnalyzeImageAltText(languageCode: "sv")]
   public virtual string AltTextSE { get; set; }
   ```

   ![image](https://github.com/user-attachments/assets/71ea9234-dbc3-4ff1-9a08-1ad8f1579ab8)


3. **Generate Tags**:
   ```csharp
   [AnalyzeImage(prompt: "Return tags from the image on objects in the picture. Return a comma-separated list.")]
   public virtual IList<string> Tags { get; set; }
   ```

   ![image](https://github.com/user-attachments/assets/1bda2d7b-cb51-4dfd-be53-2850335f53d9)


4. **Generate Sales Description**:
   ```csharp
   [AnalyzeImage(prompt: "Describe this as a persuasive sales pitch, you are selling a product.", languageCode: "en")]
   public virtual string SalesDescription { get; set; }
   ```

   ![image](https://github.com/user-attachments/assets/ff7b6a7b-d34e-49f3-9235-d4f9fd4429f8)


5. **Identify Objects**:
```csharp
    [AnalyzeImage(prompt: "Return true or false if the image contains one or more cars. Return true or false only.")]
    public virtual bool IsCarInImage { get; set; }
```
![image](https://github.com/user-attachments/assets/c25e79da-d87c-4304-81e4-8753c1e274cc)


6. **Count People**:
```csharp
    [AnalyzeImage(prompt: "How many people is there in the picture? return an int")]
    public virtual int PeopleCountTest { get; set; }
```

![image](https://github.com/user-attachments/assets/16462e18-3891-41ab-9fa5-82967ce4b835)


7. **Extract Text from Images OCR (Optical Character Recognition)**:
```csharp
    [AnalyzeImage(prompt: "Return the Text that appear in the image. Return in semi colon separated (;) text.")]
    public virtual string TextInImage { get; set; }
```

![image](https://github.com/user-attachments/assets/6556ce22-a79d-4eee-a59b-ce6a44acf7c4)
![image](https://github.com/user-attachments/assets/4bcafa11-9cea-42fc-bad0-1c0a5d30c292)


---

## Additional Configuration Notes

- **Localization**: You can specify the `languageCode` to localize the analysis results.
- **Custom Prompts**: Override the default prompt for tailored responses based on specific requirements.
- **Attribute Targets**: Ensure attributes are applied to compatible property types such as `string`,`int`,`bool`, `IList<string>`, or `IList<LocalizedString>`.

---

## Example: Custom Attribute

### Creating a Custom Attribute

You can create custom attributes for specific use cases. Below is an example of a `CopywriteAttribute` that inherits from `AnalyzeImageAttribute` and implements (not mandatory) the `INoImagePrompt` interface:

- **`INoImagePrompt`**: This interface is used for attributes that do not require an image for their response.

```csharp
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CopywriteAttribute : AnalyzeImageAttribute, INoImagePrompt
{
    /// <summary>
    /// Analyze image and create a description. Apply to string or localized string properties.
    /// </summary>
    /// <param name="languageCode">Translate description to specified language. Default English</param>
    /// <param name="prompt">Override with your own prompt</param>
    public CopywriteAttribute(string prompt = null, string languageCode = null) : base(prompt, languageCode)
    {
        base._prompt = prompt ?? $"Always return 'Epicweb AI-Assistant'";
        base._languageCode = languageCode;
    }
}
```

### Usage Example
Apply the custom attribute to a property in your media object class as shown below:

```csharp
[ContentType(GUID = "0A89E464-56D4-449F-AEA8-2BF774AB8730")]
[MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png,webp")]
public class ImageFile : ImageData
{
    [Copywrite]
    public virtual string Copyright { get; set; }

    [AnalyzeImage(prompt: "Return tags from the image on objects in the picture. Return a comma-separated list.")]
    public virtual IList<string> Tags { get; set; }

    [AnalyzeImage(prompt: "Describe this as a persuasive sales pitch, you are selling a product.", languageCode: "en")]
    public virtual string SalesDescription { get; set; }
}
```

### Key Details
- **`INoImagePrompt`**: This interface is used for attributes that do not require an image for their response.
- **`CopywriteAttribute`**: Customizes the prompt and language code for generating descriptions.

## Example extract AltText from Image

**Template:** 

```csharp
@model EPiServer.Core.ContentReference
@using EPiServer.Web.Mvc.Html
@using Epicweb.Optimizely.AIAssistant.Extensions
@if (Model != null)
{
    <figure>
        <div class="image-wrapper">
            <img class="centered-image" 
                 src="@Url.ContentUrl(Model)" 
                     alt="@Model.GetAltText("en")">
        </div>
        <figcaption>@Model.GetAltText("en")</figcaption>
    </figure>
}
```

**Extension code:**
```csharp
using AlloySandbox.Models.Media;
using EPiServer.ServiceLocation;
using Epicweb.Optimizely.AIAssistant.Extensions;

namespace AlloySandbox.Extensions
{
    public static class Extensions
    {
        private static readonly Lazy<IContentLoader> ContentLoader =
     new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        public static string GetAltText(this ContentReference contentReference, string fallbackLang)
        {
            if (ContentLoader.Value.TryGet(contentReference, out ImageFile image))
            {
                return image?.AltTextList?.GetPreferredCulture(fallbackLang) ?? string.Empty;
            }
            return string.Empty;
        }

    }
}
```

---
