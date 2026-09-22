# Shortcuts

## What are Shortcuts?

Shortcuts are reusable AI prompt actions that speed up common content editing tasks. They are one-click buttons or menu options in the Optimizely editor UI that trigger predefined AI operations like translating text, changing tone, generating SEO descriptions, and more.

**Key characteristics:**

- **Reusable** — A single shortcut can be attached to multiple properties and content types
- **Configurable** — Enable/disable shortcuts globally, per content type, or per property
- **Extensible** — Create custom shortcuts for your organization's common workflows
- **Non-intrusive** — Users can opt-in by clicking; no automatic changes unless configured

### Visual Examples

Shortcuts appear as buttons and dropdown menus in text editors:

- **Quick access buttons** in input fields (textarea, rich text)
- **Grouped menus** for related shortcuts (e.g., "Change Tone" groups tone variants)
- **Hierarchical organization** with parent shortcuts that contain child shortcuts

### Behind the Scenes

Each shortcut:

1. Is implemented as a C# class inheriting `IPromptShortcut`
2. Generates prompt text via `GeneratePrompt(...)`
3. Controls visibility via metadata (`Name`, `Enabled`, `SortOrder`, `ParentName`)
4. May include optional tool integration (`UseTools`, `Tools`)
5. Optionally implements `IJavascriptPromptShortcut` for frontend-specific behavior (e.g., setting fields, executing actions)

---

## Built-in Shortcuts

The core `Epicweb.Optimizely.AIAssistant` package includes the following shortcuts out of the box.

### Main Orchestrator Shortcuts

| Shortcut | Name in UI | Purpose | Scope |
|----------|-----------|---------|-------|
| `PromptShortcut` | Ask AI Assistant | Opens the AI prompt dialog for custom instructions | All fields |
| `GroupPromptShortcut` | More | Grouping node for additional shortcuts | All fields |

### Rewrite & Enhancement

| Shortcut | Prompt Pattern | Purpose | Scope |
|----------|----------------|---------|-------|
| `CheckSpellingPromptShortcut` | `#[SPELLCHECK]: <text>` | Spell-check and grammar enhancement | Input, textarea, RTE |
| `ElaboratePromptShortcut` | `#[ELABORATE]: <text>` | Expand text with more detail | Input, textarea, RTE |
| `FeedbackPromptShortcut` | `#[FEEDBACK]: <text>` | Request AI feedback on content | Input, textarea, RTE |
| `ShortenPromptShortcut` | `#[SHORTEN]: <text>` | Condense text while preserving meaning | Input, textarea, RTE |
| `SuggestPromptShortcut` | `#[SUGGEST]: "<text>"` or `#[TRANSLATE]` | Suggest improvements; fallback translate if empty | Input, textarea, RTE |

### Tone & Voice

Parent shortcut: **`ChangeTonePromptShortcut`** ("Change Tone" menu)

All tone shortcuts use the pattern `#[TONE]: <text>`

| Shortcut | Prompt Tone | Use Case |
|----------|------------|----------|
| `ConfidentPromptShortcut` | Confident | Assertive, strong messaging |
| `EmpatheticPromptShortcut` | Empathetic | Understanding, compassionate messaging |
| `FormalPromptShortcut` | Formal | Official, professional documents |
| `HumorPromptShortcut` | Humor | Light, entertaining tone |
| `InFormalPromptShortcut` | Informal | Casual, conversational tone |
| `OptimisticPromptShortcut` | Optimistic | Positive, forward-looking messaging |
| `PassionatePromptShortcut` | Passionate | Enthusiastic, heartfelt tone |
| `ProvocativePromptShortcut` | Provocative | Attention-grabbing, bold messaging |
| `SalesPromptShortcut` | Sales | Persuasive, marketing-focused tone |
| `SeriousPromptShortcut` | Serious | Stern, grave, formal tone |
| `SimplisticPromptShortcut` | Simplistic | Simple, clear, easy-to-understand language |
| `SkepticalPromptShortcut` | Skeptical | Questioning, doubtful tone |

### Summarization & Generation

| Shortcut | Pattern/Behavior | Purpose | Scope |
|----------|-----------------|---------|-------|
| `SummarizeShortPromptShortcut` | `#[SUMMARIZE20]: ...` with `::this::` fallback | Create brief 20-word summaries | Input, textarea, RTE |
| `SalesSumPromptShortcut` | `#[SALES]: ...` with `::this::` fallback | Generate sales pitch summary | Input, textarea, RTE |
| `GenerateLoremPromptShortcut` | Lorem ipsum generation | Generate 25-word placeholder text | Input, textarea, RTE |
| `GenerateLorem2PromptShortcut` | Lorem ipsum generation | Generate 60-word placeholder text | Input, textarea, RTE |
| `KeywordsGenerateArticlePromptShortcut` | Article generation from keywords | Create full articles from keyword input | RTE only |

### SEO & Meta

| Shortcut | Prompt Pattern | Purpose | Scope |
|----------|----------------|---------|-------|
| `SeoTitlePromptShortcut` | `#[SEOTITLE]: ...` with `::this::` fallback | Generate SEO-optimized page titles | Input, textarea |
| `SeoDescriptionPromptShortcut` | `#[SEODESC]: ...` with `::this::` fallback | Generate SEO meta descriptions | Input, textarea |
| `SeoKeywordsPromptShortcut` | `#[KEYWORDS]: ...` with `::this::` fallback | Generate relevant keywords based on content | Input, textarea |

### Translation

| Shortcut | Prompt Pattern | Purpose | Scope |
|----------|----------------|---------|-------|
| `TranslatePromptShortcut` | `#[TRANSLATE]: "<text>"` or `#[TRANSLATE]` when empty | Translate text to target language | Input, textarea, RTE |

### Rich Text Editor (RTE) Formatting & Accessibility

Parent shortcut: **`FormattingPromptShortcut`** ("Formatting" menu)

These shortcuts are RTE-only and modify HTML structure and semantics:

| Shortcut | Purpose | Scope |
|----------|---------|-------|
| `WCAGCompatPromptShortcut` | Enhance WCAG Accessibility | Improve HTML for accessibility compliance (RTE only) |
| `HeadingLevelsPromptShortcut` | Fix Heading Levels | Normalize heading hierarchy (`<h1>`, `<h2>`, etc.) (RTE only) |
| `ToCPromptShortcut` | Create Table of Contents | Generate TOC with anchors and navigation links (RTE only) |
| `FaqPromptShortcut` | Generate FAQ | Create semantic FAQ section from content (RTE only) |

---

## How to Use Shortcuts

### As an Editor

1. **Locate shortcuts** in your text field or rich text editor
   - For input/textarea: look for buttons or a dropdown menu
   - For RTE: check the toolbar for shortcut buttons/menu

2. **Select text** (optional for some shortcuts)
   - Some shortcuts work on highlighted text
   - Others work on the entire field content

3. **Click the shortcut** button or menu option
   - The AI processes your request
   - The result is inserted or suggested for approval

4. **Review and accept**
   - Compare the AI's output with your original
   - Click "Use this suggestion" to accept
   - Or undo (Ctrl+Z) if you prefer the original

### Using Hashtag Prompts

Shortcuts can also be invoked via **hashtag syntax**:

```
#[SHORTCUT_NAME]: <text>
```

Examples:

- `#[SPELLCHECK]: "your text here"` — Spell-check your text
- `#[SHORTEN]: "long text here"` — Condense text
- `#[SEOTITLE]: ::this::` — Generate SEO title based on page content
- `#[FORMAL]: "informal text"` — Rewrite in formal tone
- `#[TRANSLATE]: "english text"` — Translate to page's language

---

## Configuration

### Enable/Disable Shortcuts Per Property

Use the `[AIAssistant]` attribute on your content property:

```csharp
// Show all default shortcuts (default behavior)
[UIHint(AIHint.Textarea)]
public virtual string MyField { get; set; }

// Disable all shortcuts on this property
[UIHint(AIHint.Textarea)]
[AIAssistant(ShortcutsDisabled = true)]
public virtual string MyField { get; set; }

// Allow only specific shortcuts (allowlist)
[UIHint(AIHint.Textarea)]
[AIAssistant(Shortcuts = new[] {
	typeof(PromptShortcut),
	typeof(SuggestPromptShortcut),
	typeof(TranslatePromptShortcut),
	typeof(ShortenPromptShortcut),
	typeof(CheckSpellingPromptShortcut)
})]
public virtual string MyField { get; set; }
```

### Global Configuration

Register shortcuts in `Startup.cs` / `Program.cs`:

```csharp
services.AddAIAssistant()
	// Built-in shortcuts are auto-registered
	.RegisterMcpToolType(typeof(MyCustomTools));
```

**Disable all default shortcuts globally** (optional):

```csharp
// Remove specific shortcuts from DI
services.AddSingleton<IPromptShortcut, YourCustomShortcut>();
```

**Disable AutoSuggest globally** (prevents automatic suggestions on field leave):

```json
{
  "Epicweb": {
	"AIAssistant": {
	  "AutoSuggest": false
	}
  }
}
```

---

## Creating Custom Shortcuts

### Basic Structure

```csharp
using Epicweb.Optimizely.AIAssistant.Shortcuts;

public class MyCustomShortcut : IPromptShortcut
{
	public string Name => "My Custom Action";
	public int SortOrder => 100; // Higher = lower priority
	public string? ParentName => null; // null = root level
	public bool Enabled => true;
	public bool EnabledInRichTextEditor => true;
	public string? EmptyMessage => "Please enter text first";
	public string? Tools => null;
	public bool UseTools => false;

	public string GeneratePrompt(string userInput, string? selectedText, Dictionary<string, object>? contextData)
	{
		// Build your prompt here
		return $"My instruction: {userInput}";
	}
}
```

### Register Custom Shortcuts

In `Startup.cs`:

```csharp
services.AddAIAssistant();
services.AddSingleton<IPromptShortcut, MyCustomShortcut>();
```

### Group Custom Shortcuts

Create a parent shortcut to organize related ones:

```csharp
public class MyGroupShortcut : IPromptShortcut
{
	public string Name => "My Group";
	public int SortOrder => 50;
	public string? ParentName => null;
	public bool Enabled => true;
	// ... other properties
}

public class MyChildShortcut : IPromptShortcut
{
	public string Name => "Child Action";
	public int SortOrder => 10;
	public string? ParentName => nameof(MyGroupShortcut); // Reference parent
	// ... other properties
}
```

### Using Tools with Shortcuts

Shortcuts can invoke AI tools for dynamic content:

```csharp
public class MyToolShortcut : IPromptShortcut
{
	public string? Tools => "GetContentById,GetThisContent"; // Comma-separated
	public bool UseTools => true;

	public string GeneratePrompt(string userInput, string? selectedText, Dictionary<string, object>? contextData)
	{
		return $"Use the available tools to {userInput}";
	}
}
```

---

## Common Scenarios

### Scenario 1: SEO Title Field — Limited Shortcuts

```csharp
[Display(Name = "SEO Title")]
[UIHint(AIHint.Input)]
[AIAssistant(Shortcuts = new[] {
	typeof(SeoTitlePromptShortcut),
	typeof(ShortenPromptShortcut)
})]
public virtual string SeoTitle { get; set; }
```

**Result:** Only "SEO Title" and "Shorten" shortcuts appear on this field.

### Scenario 2: Rich Description — Full Shortcuts + Custom

```csharp
[Display(Name = "Rich Description")]
[UIHint("XhtmlString")]
[AIAssistant()] // All default shortcuts
public virtual XhtmlString Description { get; set; }

// In Startup.cs
services.AddSingleton<IPromptShortcut, GenerateBulletPointsShortcut>();
```

**Result:** All default shortcuts plus your custom "Generate Bullet Points" action.

### Scenario 3: Internal Notes — No Shortcuts

```csharp
[Display(Name = "Internal Notes")]
[UIHint(AIHint.Textarea)]
[AIAssistant(ShortcutsDisabled = true)]
public virtual string InternalNotes { get; set; }
```

**Result:** No shortcuts available on this field.

---

## Troubleshooting

### Shortcuts Don't Appear

1. **Check `[AIAssistant]` configuration**
   - Ensure `ShortcutsDisabled` is not `true`
   - If using an allowlist, verify the shortcut type is included

2. **Check global registration**
   - Confirm shortcuts are registered in `Startup.cs` via `services.AddAIAssistant()`
   - Verify shortcut's `Enabled` property is `true`

3. **Check field UIHint**
   - Only `AIHint.Input`, `AIHint.Textarea`, and RTE fields support shortcuts
   - Rich text editors (XhtmlString) have a subset of shortcuts

### Shortcut Produces Wrong Output

1. Check the shortcut's `GeneratePrompt()` implementation
2. Verify the AI model has appropriate instructions via `[AIAssistant(AssistantInstructions = "...")]`
3. Test with a different AI provider or model

### Custom Shortcut Not Registered

1. Ensure the class implements `IPromptShortcut`
2. Verify registration in `Startup.cs`: `services.AddSingleton<IPromptShortcut, YourShortcut>()`
3. Check that `Enabled` property is `true`
4. Clear browser cache and restart the application

---

## Related Documentation

- [Configuration Guide](./configuration.md) — Detailed attribute and appsettings options
- [User Manual](./user-manual.md) — How editors use shortcuts and prompts
- [Example Screens](./example-screens.md) — Visual examples of shortcuts in the editor
- [Chat Instructions](./chat-instructions.md) — Building custom instructions with shortcuts
- [Shortcut Implementer Guide](./promptshortcuts.md) — In-depth developer guide for extending shortcuts
