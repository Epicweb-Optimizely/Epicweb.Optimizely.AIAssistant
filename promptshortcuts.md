# Shortcut implementer guide

This guide is for developers integrating or extending `Epicweb.Optimizely.AIAssistant` in Optimizely.

## What a shortcut is

A shortcut is a reusable prompt action shown in the editor UI.

![image](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/bc57c229-802d-45bb-a59a-f07613070a6f)

- It is implemented as a class that implements `IPromptShortcut`.
- It builds prompt text in `GeneratePrompt(...)`.
- It is reused across multiple fields and content types.

Each shortcut controls:

- metadata (`Name`, `SortOrder`, `ParentName`)
- visibility (`Enabled`, `EnabledInRichTextEditor`)
- invalid-input feedback (`EmptyMessage`)
- optional tool behavior (`Tools`, `UseTools`)

Some shortcuts also implement `IJavascriptPromptShortcut` to influence frontend action (`aiprompt`, `setfield`, `execute_ai`).

## How shortcuts connect to fields

Shortcuts are attached through AI-enabled property editors and `[AIAssistant(...)]`.

Typical field setup:

- use `UIHint` (`AIHint.Input`, `AIHint.Textarea`, or RTE/XhtmlString usage)
- configure the property with `[AIAssistant]`

Per-property shortcut behavior:

- `[AIAssistant(ShortcutsDisabled = true)]` → hide all shortcuts on that field
- `[AIAssistant(Shortcuts = new[] { typeof(SeoKeywordsPromptShortcut) })]` → allow-list selected shortcuts only
- no `Shortcuts` specified → show registered shortcuts where `Enabled` is `true`

## Adding custom shortcuts

1. Create a class implementing `IPromptShortcut`.
2. Implement `GeneratePrompt(...)` with your organization’s reusable prompt format.
3. Register the class in DI as `IPromptShortcut`.
4. Optionally expose it only on specific fields using `AIAssistantAttribute.Shortcuts`.

## Disabling default shortcuts

You can disable defaults at different levels:

- global: remove the `services.AddSingleton<IPromptShortcut, ...>()` registration
- class level: set `Enabled` / `EnabledInRichTextEditor` defaults to `false`
- property level: `ShortcutsDisabled = true`
- property allow-list: explicit `Shortcuts = ...`

## Built-in shortcuts in the core package (`Epicweb.Optimizely.AIAssistant`)

### Grouping / parent shortcuts

- `PromptShortcut` (`Ask AI Assistant`) — opens AI prompt dialog and forwards text
- `ChangeTonePromptShortcut` (`Change Tone`) — parent for tone shortcuts
- `FormattingPromptShortcut` (`Formatting`) — parent for formatting/accessibility shortcuts
- `GroupPromptShortcut` (`More`) — additional grouping node

### Rewrite and enhancement shortcuts

- `CheckSpellingPromptShortcut` — `#[SPELLCHECK]: <text>`
- `ElaboratePromptShortcut` — `#[ELABORATE]: <text>`
- `FeedbackPromptShortcut` — `#[FEEDBACK]: <text>`
- `ShortenPromptShortcut` — `#[SHORTEN]: <text>`
- `SuggestPromptShortcut` — `#[SUGGEST]: "<text>"` (fallback `#[TRANSLATE]` in non-RTE empty-input case)

### Tone shortcuts (children of `ChangeTonePromptShortcut`)

- `ConfidentPromptShortcut` — `#[CONFIDENT]: <text>`
- `EmpatheticPromptShortcut` — `#[EMPATHETIC]: <text>`
- `FormalPromptShortcut` — `#[FORMAL]: <text>`
- `HumorPromptShortcut` — `#[HUMOR]: <text>`
- `InFormalPromptShortcut` — `#[INFORMAL]: <text>`
- `OptimisticPromptShortcut` — `#[OPTIMISTIC]: <text>`
- `PassionatePromptShortcut` — `#[PASSIONATE]: <text>`
- `ProvocativePromptShortcut` — `#[PROVOCATIVE]: <text>`
- `SalesPromptShortcut` — `#[SALES]: <text>`
- `SeriousPromptShortcut` — `#[SERIOUS]: <text>`
- `SimplisticPromptShortcut` — `#[SIMPLISTIC]: <text>`
- `SkepticalPromptShortcut` — `#[SKEPTICAL]: <text>`

### Summarize / generate shortcuts

- `SummarizeShortPromptShortcut` (`Summarize short`) — `#[SUMMARIZE20]: ...` (`::this::` fallback)
- `SalesSumPromptShortcut` (`Summarize Sales Pitch`) — `#[SALES]: ...` (`::this::` fallback)
- `GenerateLoremPromptShortcut` — lorem ipsum (25 words)
- `GenerateLorem2PromptShortcut` — lorem ipsum (60 words)
- `KeywordsGenerateArticlePromptShortcut` — article generation from keyword input

### SEO shortcuts

- `SeoTitlePromptShortcut` — `#[SEOTITLE]: ...` (`::this::` fallback)
- `SeoDescriptionPromptShortcut` — `#[SEODESC]: ...` (`::this::` fallback)
- `SeoKeywordsPromptShortcut` — `#[KEYWORDS]: ...` (`::this::` fallback)

### Translation shortcut

- `TranslatePromptShortcut` — `#[TRANSLATE]: "<text>"` (or `#[TRANSLATE]` when empty)

### RTE formatting/accessibility shortcuts (children of `FormattingPromptShortcut`)

These are internal and are for examples use

- `WCAGCompatPromptShortcut` (`Enhance WCAG Accessibility`) — RTE-only WCAG-focused HTML rewrite
- `HeadingLevelsPromptShortcut` (`Fix heading levels`) — RTE-only heading normalization
- `ToCPromptShortcut` (`Create Table of Content`) — RTE-only TOC generation with anchors/nav
- `FaqPromptShortcut` (`Generate FAQ`) — RTE-only semantic FAQ section generation

## Notes

- This document intentionally lists core package shortcuts only.
- Core shortcuts rely on tagged prompt patterns (`#[...]`) and shared assistant pipeline behavior.
