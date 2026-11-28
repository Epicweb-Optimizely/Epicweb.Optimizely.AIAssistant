# Epicweb SEO Manager for Optimizely CMS

SEO Manager is part of Epicweb AI Assistant. It lets editors scan a site or a section of the page tree for missing or overly long SEO titles and meta descriptions, review AI suggestions based on the page content, and save the changes as Optimizely drafts.

## Screen shots

### Scanning site

<img width="1382" height="749" alt="image" src="https://github.com/user-attachments/assets/a11ef6a0-3102-4cb5-a794-2ce884cb51be" />


### Review the list, one page at the time

<img width="1384" height="881" alt="image" src="https://github.com/user-attachments/assets/885f6f90-9684-4926-abbe-d7b2728f7a64" />

### Review the SEO fields

<img width="386" height="815" alt="image" src="https://github.com/user-attachments/assets/92bd894f-4ad5-41e7-9591-3f4c08146bd3" />


## Enable SEO Manager

SEO Manager works with AI Chat turned off. If this is the site's first AI Assistant feature, install the `Epicweb.Optimizely.AIAssistant` NuGet package for your CMS version and register it in startup. If the site already calls `AddAIAssistant()`, reuse that registration. It includes SignalR and the background scan service. See the [package installation guide](installation.md).

```csharp
using Epicweb.Optimizely.AIAssistant;

// In ConfigureServices
services.AddAIAssistant();
```

Merge the following setting into `appsettings.json`. You can leave `EnableChat` at its default of `false`.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "EnableSeoManager": true
    }
  }
}
```

Before running a scan, [map the site's SEO properties](#map-your-existing-seo-properties). Scanning checks those fields locally. To use **Analyse and suggest** or **Generate**, also configure the site's AI provider and credentials.

### Configure live scan updates

SEO Manager and AI Chat share the same SignalR hub. If the site already calls `MapAIAssistantHub(...)` for chat, that mapping also serves SEO Manager. On a site without a hub mapping, add this line to the existing `app.UseEndpoints(endpoints => { ... })` block for live updates.

```csharp
endpoints.MapAIAssistantHub();
```

The default path is `/epicweb-progresshub`. For a custom path, use `endpoints.MapAIAssistantHub("/edit/epicweb-hub");` in place of the call above. Passing the path to the method configures both the server route and the URL sent to the browser. Setting `SignalRHubPath` in `appsettings.json` alone leaves the server route at its default.

The browser fetches scan status and results over HTTP every two seconds. A hub notification triggers an earlier refresh. Scans therefore still work when the hub is unavailable. To use WebSockets, enable WebSocket support on the host and allow connection upgrades through any reverse proxy on the hub path. The SEO Manager view loads the SignalR client from `cdnjs.cloudflare.com`, so the editor's browser and the site's content security policy must allow that script for live updates.

On a load-balanced CMS, route the editor's scan requests to the same application instance using session affinity. Each instance keeps its own scan jobs and results in memory. Review and ignore marks are also local to the instance and are cleared when it restarts.

## Content permissions

Access to SEO Manager lets users open the feature. Optimizely checks the user's CMS permissions on each page.

Users in `AIEditors` or `WebAdmins` can open **SEO Manager** from the **AI Assistant** menu. Grant additional roles access through `Epicweb:AIAssistant:AIAssistantSEOManager`. The shared hub accepts `AIEditors`, `WebEditors`, `WebAdmins`, `CmsAdmins`, and `CmsEditors`. A user with only a custom SEO role receives scan updates through HTTP polling. See [Granting SEO Manager access](admin-features.md#granting-seo-manager-access).

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

### Description fallbacks

Declare fallbacks on the mapped metadata property, in the order your site uses them. A base page model can include fields that exist only on specific page types.

```csharp
[CultureSpecific]
[SeoField(SeoFieldType.Description, nameof(TeaserText), nameof(StandardPage.Excerpt))]
public virtual string MetaDescription { get; set; }
```

In this example, both page types inherit `MetaDescription` and `TeaserText`. Only `StandardPage` defines `Excerpt`.

| Page type | Fields checked in order |
| --- | --- |
| `StandardPage` | `MetaDescription`, `TeaserText`, `Excerpt` |
| `LandingPage` | `MetaDescription`, `TeaserText` |

Scan and Review use the first value containing text on the selected language version. SEO Manager skips fallback properties absent from the current page type. Each fallback property present on that type must be a publicly readable, non-indexed `string` distinct from the mapped field.

Review shows the active fallback source. Saving an override updates `MetaDescription`. Clearing it restores the fallback. To render the same description in the public page template, use `ISeoFieldResolver.Resolve(type).GetEffectiveValue(page, SeoFieldType.Description)`.

An empty effective description is missing. A description of 1 to 69 trimmed characters gets the editorial reminder **Review description**. This reminder allows saving and publishing.

## Customize AI suggestions

For each suggestion request, SEO Manager first loads the general assistant instructions through `IAssistantInstructionsResolver.GetInstructions(page.Language)`. It then appends the SEO metadata instructions and response rules. Suggested metadata uses the page language. Analysis comments use the editor's preferred UI language, with English as the default.

To customize the SEO instructions, override `Instructions/SeoManager/Metadata.md` or add a language-specific file such as `Metadata.sv.md`. See the [SEO Manager metadata prompt setup](chat-instructions.md#seo-manager-metadata-prompts).

## Find pages and review metadata

1. Open **SEO Manager** in the **AI Assistant** menu. Choose a site and language, narrow the scope by subtree or page type as needed, and click **Find pages**.
2. Open a result to review its metadata. Edit the draft values directly or use **Analyse and suggest** to review suggestions for the mapped fields. Use **Generate** or **Regenerate** to request a suggestion for an individual field.
3. Click **Use suggestion** to copy a suggestion into the draft value, then click **Save draft**. Users with publish access can publish the saved draft from SEO Manager after the language version has been published at least once in the CMS.
