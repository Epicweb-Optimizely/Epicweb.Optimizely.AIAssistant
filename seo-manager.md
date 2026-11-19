# Epicweb SEO Manager for Optimizely CMS

SEO Manager is part of Epicweb AI Assistant. It lets editors scan a site or a section of the page tree for missing or overly long SEO titles and meta descriptions, review AI suggestions based on the page content, and save the changes as Optimizely drafts.

## Enable SEO Manager

In an application that already uses Epicweb AI Assistant, set `Epicweb:AIAssistant:EnableSeoManager` to `true` in `appsettings.json`. This adds **SEO Manager** to the AI Assistant menu for users with access.

## Map your existing SEO properties

Import `Epicweb.Optimizely.AIAssistant` and annotate the existing properties on your page models that the site uses for SEO metadata. For example, a page model with language-specific fields could use the following attributes.

```csharp
[CultureSpecific]
[SeoField(SeoFieldType.Title)]
public virtual string MetaTitle { get; set; }

[CultureSpecific]
[SeoField(SeoFieldType.Description)]
public virtual string MetaDescription { get; set; }
```

Each mapped property must be a readable and writable `string`. Map each `SeoFieldType` at most once per page type. If your site also uses a keywords field, you can map it with `[SeoField(SeoFieldType.Keywords)]`.

## Customize AI suggestions

SEO Manager generates suggestions in the page's language. To adapt the wording to your brand or market, override `Instructions/SeoManager/Metadata.md` or add a language-specific file such as `Metadata.sv.md`. See the [SEO Manager metadata prompt setup](chat-instructions.md#seo-manager-metadata-prompts).
