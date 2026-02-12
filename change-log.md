# Change log

## 3.0.0 - 2026-02-12
- **NEW MAJOR FEATURE: AI Chat Window** - Standalone conversational AI assistant integrated into Optimizely CMS
  - **Chat Interface** - Floating, resizable chat window accessible from CMS toolbar
  - **Context Management** - Drag-and-drop content attachment with context overlay UI
  - **Session History** - Persistent conversation history with message management
  - **Tool Packs** - Modular tool loading system (Work Content, Update, Publish, Create, Image)
  - **Lazy Loading** - Efficient context and tool loading for optimal performance
  - **@ Mentions** - Autocomplete tool discovery in chat and property editors
  - **Chat Settings** - User preferences for chat behavior and display
  - **Feedback System** - Built-in feedback and thumbs up/down for chat responses
  - **Multi-language Support** - Full localization support for chat UI
  - **Instruction System** - Priority-based instruction hierarchy for chat behavior customization
- **NEW FEATURE: Chat Instructions** - Customizable AI behavior through markdown instruction files
- **NEW FEATURE: Context Overlay Manager** - Visual interface for managing attached content and context
- **NEW FEATURE: Tool Pack System** - Group related tools with instructions for better AI understanding
- **NEW FEATURE: Chat Suggestions** - Quick action suggestions based on conversation context
- **ENHANCED: Tool System** - Expanded tool capabilities specifically for chat scenarios
- **BREAKING CHANGE: Removed Dalle-3 support** - Use Recraft AI, Gemini, gpt-image-1.5 instead
- Improved tool discovery with @ mention functionality in property editors
- New chat storage manager for session persistence


## 2.0.4 - 2026-01-25
- New FEATURE: The Image Analyzing Service can now run on additional media types that inherit from MediaData, for example QBank media types.

## 2.0.3 - 2026-01-20
- New FEATURE: Recommended models for Azure and OpenAI after retirement of GPT-4o => GPT-5.2, GPT-image-1.5
- Bugfix, issue with image generation now showing images
- Bugfix, issue saving image with to long names. Alt-text generation looping, impact high.

## 2.0.0 - 2025-10-08
- NEW FEATURE: Tools for AI - use external tools with AI with Function calling or MCP
- NEW FEATURE: Add tools to your shortcuts

## 1.17.4 - 2025-10-09
- Bugfix, dependency issue
  
## 1.17.3 - 2025-09-09
- NEW FEATURE: Gemini Nano Banana (gemini-2.5-flash-image-preview)
- Bugfix, bug in translation functionality when working with websites missing wildcard (*) in configuration
- Bugfix, missing button label in Ask AI dialog

## 1.17.2 - 2025-09-07
- Bugfix, naming of images is automatic with AI.
- Bugfix, when translations error, return errormessage + original input.
  
## 1.17.0 - 2025-06-09
- NEW FEATURE: Google Gemini AI Provider, use Google Gemini AI as a provider for AI Assistant + Google "Imagen"
  
## 1.16.0 - 2025-04-25
- NEW FEATURE: Use AI to refine and generate a detailed prompt
  
## 1.15.0 - 2025-03-06
- NEW FEATURE: New image features, replace background, remove background, generate variations.
- Implement loader in the UI image download
- Bugfix, Ask AI some times returns text instead of html.
- Bugfix, duplicate download of image in tinymce

## 1.14.2 - 2025-02-04
- NEW FEATURE: DisabledSites - disable functionality on specific sites in solution
- Bugfix, download image RTE

## 1.14.1 - 2025-01-20
- Bugfix, Ask AI not opening the dialog

## 1.14.0 - 2025-01-14
- NEW FEATURE: Smart Image Analyzer, advanced AI capabilities to analyze images and automatically populate media object properties. When images are uploaded into Optimizely CMS.
- NEW FEATURE: DBLocalizationProvider AI Assistance

## 1.13.6 - 2024-12-01
- NEW FEATURE: Recraft AI Image Generation, style and sizes in the ui added + new actions images, remove background

## 1.13.0 - 2024-11-10
- NEW FEATURE: Recraft AI Image Generation

## 1.12.1 - 2024-10-29
- The UI is now localized in nl, da
- Bugfix, issue caused in admin user interface

## 1.12 - 2024-10-22
- The UI is now localized in en, sv, dk, nb, fi, fr, de, es
- Refactor Startup registrations

## 1.11 - 2024-09-25
- Options Pattern in startup
- New shortcut SEO Description
- Bugfixes when use EnableOnAllInputs

## 1.10 - 2024-08-23
- NEW FEATURE: Custom AI - connect any AI to the service
- NEW FEATURE: Customize AI instruction per translation language
- NEW FEATURE: Customize AI model per translation language
- NEW FEATURE: IPreProcessAiRequest - Represents an interface for pre-processing AI request data before sending it to AI.
- Size dialogs
- Error message in pagetype properties of block type when in "Create state"
- Translations not working in properties of block type 
- Better error handling when using placeholders (::this::)
- Known Issue: Properties and Required properties in "Create state" on pages are not translatable with the AI in this state. 

## 1.9.1 - 2024-05-05
- NEW FEATURE: User roles can now be assigned globally or on a per-property basis to manage access to AI-Assistant
- Added shortcuts: Easyread, Formatting, WCAG, TOC, LoremIpsum, generate from keywords, Headingslevels prompts

## 1.8.2 - 2024-04-03
- ENHANCED FEATURE: Ask AI window supports now history and more useful features
- bugfixes ask ai prompt + image prompts + errorhandling

## 1.7 - 2024-03-20
- NEW FEATURE: Enable Minimal mode and disable AutoSuggest on input fields

## 1.6 - 2024-02-12
- NEW FEATURE: Image Variation with GPT-4-Vision and Dalle-3
- NEW FEATURE: Post process images
- Support for custom Edit UI path
- **End of support for Dalle-2**

## 1.5 - 2024-01-24
- NEW FEATURE: Ask AI new prompt window
- Epicweb.Optimizely.AIAssistant.LanguageManager is released version 1.1 => bugfix when translating "Questions" 

## 1.4 - 2023-11-19
- NEW FEATURE: Introducing Dalle-3 (removed in version 3.0.0 - use Recraft AI or gpt-image-1.5 or later)
- NEW FEATURE: Add more shortcuts out of the box (SEO Title Prompt)

## 1.3 - 2023-10-23
- Compatibility with Episerver.Labs.LanguageManager
- Epicweb.Optimizely.AIAssistant.LanguageManager is released version 1.0

## 1.2 - 2023-09-27
- NEW FEATURE: Add Provider for Azure OpenAI Services
- Add more shortcuts out of the box (Change tone)

## 1.1 - 2023-08-29
- NEW FEATURE: Add AI to Rich Text Editor
- NEW FEATURE: Add shortcuts
