# Epicweb SEO Manager for Optimizely CMS

SEO Manager is part of Epicweb AI Assistant. It lets editors scan a site or a section of the page tree for missing or overly long SEO titles and meta descriptions, review AI suggestions based on the page content, and save the changes as Optimizely drafts.

## Enable SEO Manager

In an application that already uses Epicweb AI Assistant, set `Epicweb:AIAssistant:EnableSeoManager` to `true` in `appsettings.json`. Users in `AIEditors` or `WebAdmins` can then open **SEO Manager** from the **AI Assistant** menu. To grant access to additional roles such as `WebEditors`, configure `Epicweb:AIAssistant:AIAssistantSEOManager`. See [Granting SEO Manager access](admin-features.md#granting-seo-manager-access) for an example.

## Content permissions

Access to SEO Manager lets users open the feature. Optimizely checks the user's CMS permissions on each page.

| Action | Required CMS permissions |
| --- | --- |
| Browse the page tree, scan pages, view metadata, or generate suggestions | `Read` |
| Save a draft, mark a page as reviewed, ignore a field, or restore an ignored field | `Read` and `Edit` |
| Publish a saved draft from SEO Manager | `Read` and `Publish`. The language version must have been published at least once in the CMS. |

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

Each mapped property must be a readable and writable `string`. Map each `SeoFieldType` at most once per page type. Pages without mapped SEO properties are skipped. The scan reads the common draft for the selected language when one exists, with the published version as a fallback. It checks mapped titles for missing values and lengths over 60 characters, and mapped descriptions for missing values and lengths over 160 characters. You can map an optional keywords field with `[SeoField(SeoFieldType.Keywords)]`.

## Find pages and review metadata

1. Open **SEO Manager** in the **AI Assistant** menu. Choose a site and language, narrow the scope by subtree or page type as needed, and click **Find pages**.
2. Open a result to review its metadata. Edit the draft values directly or use **Analyse and suggest** to review suggestions for the mapped fields. Use **Generate** or **Regenerate** to request a suggestion for an individual field.
3. Click **Use suggestion** to copy a suggestion into the draft value, then click **Save draft**. Users with publish access can publish the saved draft from SEO Manager after the language version has been published at least once in the CMS.

## Customize AI suggestions

SEO Manager generates suggestions in the page's language. To adapt the wording to your brand or market, override `Instructions/SeoManager/Metadata.md` or add a language-specific file such as `Metadata.sv.md`. See the [SEO Manager metadata prompt setup](chat-instructions.md#seo-manager-metadata-prompts).
