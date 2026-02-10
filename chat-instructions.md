# AI Chat - Getting Started Guide

The AI Chat is a powerful conversational interface that brings AI assistance directly into your Optimizely CMS editing experience. It allows editors to have natural conversations with AI while working with content, pages, blocks, and media.

## What is AI Chat?

AI Chat is a standalone chat window that can be opened from anywhere in the Optimizely CMS interface. It provides:

- **Conversational AI assistance** - Ask questions and get intelligent responses
- **Context-aware help** - Understands the content you're working with
- **Content operations** - Create, update, publish, and analyze content through conversation
- **Tool integration** - Access to powerful AI tools for content management
- **Multi-language support** - Works with all your site languages
- **Persistent history** - Conversations are saved per session

## Opening the Chat

### From the CMS Interface

1. Look for the **AI Chat** button in the Optimizely CMS toolbar
2. Click the button to open the chat window
3. The chat opens in a floating window that you can move and resize

### Chat Window Features

- **Movable and resizable** - Position it anywhere on your screen
- **Minimizable** - Hide it when not needed
- **Persistent** - Stays open while you navigate through CMS
- **Session-based** - History is maintained during your work session

## Basic Chat Usage

### Starting a Conversation

1. Type your question or request in the input field at the bottom
2. Press **Enter** or click the **Send** button
3. The AI will process your request and respond

### Example Questions

```
"Create an SEO title for this page"
"Translate the main body to Spanish"
"Summarize all child pages"
"What are the current page properties?"
"Generate alt text for images on this page"
```

### Best Practices

- **Be specific** - Clear requests get better results
- **Use context** - Attach content or set context for better understanding
- **Ask for clarification** - The AI can explain its suggestions
- **Review changes** - Always review before publishing

## Context Management

The chat can understand and work with content through the **Context System**.

### Setting Context

#### Automatic Context
When you open chat from a content item, it automatically becomes the context.

#### Manual Context (Using Context Overlay)
1. Click the **status text** in the chat header (shows current context/attachments)
2. The **Context Overlay** opens
3. Drag content from CMS or use the context drop zone
4. It auto saves when you close the overlay

### Context File vs Attached Files

- **Context File** (optional) - The main content the AI focuses on
- **Attached Files** - Additional reference materials

### Context Overlay Operations

#### Set Context File
- Drag a file to the "Drop context file here" zone
- OR click the ✓ (checkmark) button on any attached file

#### Add Attached Files
- Drag files from CMS while overlay is closed → Added directly
- Drag files while overlay is open → Staged for preview
- Click **Save Changes** to apply

#### Reorder Files
- **Drag & drop** - Use the handle (≡) to reorder
- **Arrow buttons** - Use ⬆️ and ⬇️ to move one position
- Files at the top have higher priority for AI processing

#### Remove Files
- Click trash icon (🗑️) on individual files
- Click **Clear All** to remove all attached files (keeps context)

### Why Use Context?

Setting context helps the AI:
- Understand what you're working with
- Provide more accurate suggestions
- Perform operations on the right content
- Maintain focus throughout the conversation

## Using AI Tools

AI Tools extend the chat's capabilities with specific functions. They work like having expert helpers in your conversation.

### What are Tools?

Tools are C# functions that the AI can call to:
- Fetch real CMS data
- Perform content operations
- Analyze and process information
- Connect to external systems

### Built-in Tool Packs

The AI Assistant comes with several tool packs:

| Tool Pack | Purpose |
|-----------|---------|
| **Core Tools** | Always available - basic content operations |
| **Work Content** | Read and search content, get properties |
| **Update** | Update content properties and fields |
| **Publish** | Publishing and scheduling operations |
| **Create** | Create new pages, blocks, and content |
| **Image** | Image analysis and manipulation |

### Loading Tool Packs

#### Automatic Loading
The AI can automatically load tools when needed:
```
You: "Update the heading on this page"
AI: [Loads Update tool pack and executes]
```

#### Manual Loading
You can explicitly ask to load tools:
```
"Load the Image tools"
"Show available tool packs"
"Load Update and Publish tools"
```

### Tool Discovery

#### List Available Tools
```
"What tools are available?"
"Show me all tool packs"
"List content tools"
```

#### Using @ Mentions (Property Editors)
In property fields with AI assistance:
1. Type `@` in the prompt field
2. A dropdown shows available tools
3. Select a tool to include it in your prompt
4. The tool name is inserted (e.g., `@GetContentById 5`)

### Example Tool Usage

#### Content Reading
```
"Get the MainBody property from this page"
→ Uses GetContentProperty tool

"Show all child pages"
→ Uses GetChildContent tool

"Find pages named 'Products'"
→ Uses GetContentByName tool
```

#### Content Updates
```
"Update the page heading to 'Welcome to Our Site'"
→ Uses UpdateContentProperty tool

"Change the preamble text"
→ Uses content update tools
```

#### Publishing
```
"Publish this page"
→ Uses PublishContent tool

"Schedule this page for tomorrow at 10am"
→ Uses scheduling tools
```

## Chat Instructions System

The AI behavior is controlled by a priority-based instruction system.

### Instruction Categories

| Category | Purpose | Example |
|----------|---------|---------|
| **Global** | Overall chat behavior | How to respond, tone, guidelines |
| **Context** | How to handle context | When to fetch content, what to include |
| **General** | Content editing guidelines | SEO, accessibility, formatting |
| **ContentType** | Page/block specific rules | Instructions for ProductPage, NewsPage |
| **Field** | Property-specific guidance | Rules for specific properties |

### How Instructions Work

1. **Built-in Instructions** - Default behavior from the plugin
2. **Custom Instructions** - Your project-specific guidelines
3. **Priority-based** - Higher priority instructions override lower ones

### Customizing Chat Instructions

You can customize how the AI behaves in your project:

#### 1. Create Instruction Files

Create markdown files in your project:
```
YourProject/
  Instructions/
    chat/
      global.md          # Overall chat behavior
      general.md         # General guidelines
      context.md         # Context handling
    ContentType/
      ProductPage.md     # Product-specific rules
    Field/
      MainBody.md        # Field-specific rules
```

#### 2. Mark as Embedded Resources

In your `.csproj`:
```xml
<ItemGroup>
  <EmbeddedResource Include="Instructions\**\*.md" />
</ItemGroup>
```

#### 3. Register in Startup

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAIAssistant()
           .AddEmbeddedInstructionStore(
               Assembly.GetExecutingAssembly(),
               priority: 20  // Higher priority overrides defaults
           );
}
```

### Example Custom Instruction

**Instructions/chat/global.md**
```markdown
# Company Chat Guidelines

You are assisting editors at [Your Company Name].

## Brand Voice
- Always maintain a professional yet friendly tone
- Use our brand terminology: "solutions" not "products"
- Emphasize sustainability in all content

## Content Rules
- All product pages must include eco-friendly certifications
- SEO descriptions should be exactly 155 characters
- Always suggest image alt text for accessibility

## Response Format
- Provide concise answers
- Include examples when helpful
- Ask before making changes
```

## Message History

### Viewing History

The chat maintains conversation history:
- **Current session** - All messages since opening chat
- **Previous messages** - Available when reopening chat in same session
- **System messages** - Shows what the AI did (tool calls, context changes)

### History Features

- **Auto-scroll** - Latest messages automatically visible
- **Timestamps** - Each message shows when it was sent
- **Tool indicators** - See when AI used tools (ℹ️ icons)
- **User preferences** - Settings persist across sessions

### Clearing History

```
"Clear chat history"
"Start a new conversation"
```

## Chat Settings

### User Preferences

Access settings by clicking the **settings/gear icon** in the chat window header.

#### Available Settings

**Display System Messages**
- **Purpose**: Show or hide technical system messages and tool execution details
- **Default**: Enabled
- **Use case**: Enable to see what tools the AI is using and how it's processing requests. Disable for a cleaner chat experience.

**Custom Instructions**
- **Purpose**: Override default AI instructions for the current chat session
- **Persistence**: Saved only in the current session (uses sessionStorage)
- **Use case**: Provide session-specific context or behavior without changing global settings
- **Example**: "Focus on SEO optimization" or "Always suggest WCAG compliance improvements"

**Console Debug Mode**
- **Purpose**: Enable detailed logging to browser console for troubleshooting
- **Default**: Disabled
- **Use case**: Enable when debugging issues or when providing feedback to support
- **Location**: Logs appear in browser Developer Tools Console (F12)

### How to Access Settings

1. Open the chat window
2. Click the **⚙️ Settings** icon in the chat header
3. Adjust preferences as needed
4. Settings are saved automatically when you change them

### Custom Instructions Usage

When you first start a chat, the default instructions populate the Custom Instructions field. You can:
- **View** the current instructions being used
- **Edit** them to customize AI behavior for this session
- **Clear** them to revert to defaults
- Instructions are **session-specific** - they don't affect other users or persist after browser refresh

**Example Custom Instructions:**
```
Focus on creating accessible content that meets WCAG 2.1 AA standards.
Always suggest alt text for images.
Keep SEO descriptions between 150-160 characters.
Use our brand voice: professional but friendly.
```

### Debug Mode Details

When Console Debug Mode is enabled, you'll see detailed logs including:
- API requests and responses
- Tool loading and execution
- Context changes
- Storage operations
- Error details and stack traces

**How to use debug logs:**
1. Enable Console Debug Mode in settings
2. Open browser Developer Tools (F12)
3. Go to the Console tab
4. Perform actions in the chat
5. Review detailed logs with timestamps

### Settings Persistence

| Setting | Persistence | Scope |
|---------|-------------|-------|
| Display System Messages | Saved across sessions | Per user browser |
| Custom Instructions | Current session only | Session-specific |
| Console Debug Mode | Saved across sessions | Per user browser |

**Note:** Custom Instructions use `sessionStorage` which means:
- ✅ Persists during the current browser session
- ✅ Survives page navigation within CMS
- ❌ Cleared when browser tab/window is closed
- ❌ Not shared across browser tabs

## Advanced Features

### Working with Selected Text

1. Select text in a content property
2. Open chat
3. The selected text becomes available to the AI
4. Ask to improve, translate, or modify it

### Bulk Operations

```
"Update SEO descriptions on all child pages"
"Translate all product pages to German"
"Generate alt text for all images in media folder"
```

### Content Analysis

```
"Check accessibility of this page"
"Analyze readability score"
"Compare this page to our competitors"
"Review SEO optimization"
```

### Integration with Property Editors

The chat works alongside property-level AI assistance:
- Use chat for **complex multi-step tasks**
- Use property AI for **quick single-field improvements**
- Both share the same AI capabilities and instructions

## Troubleshooting

### Chat Not Responding

1. **Check API key** - Verify configuration in `appsettings.json`
2. **Check network** - Ensure connection to AI provider
3. **Check logs** - Look for errors in application logs
4. **Try again** - Sometimes a simple retry works

### Context Issues

1. **Verify content is attached** - Check context overlay
2. **Clear and re-add** - Remove and add content again
3. **Check permissions** - Ensure you have read access to content

### Tool Loading Fails

1. **Check tool registration** - Verify tools are registered in `Startup.cs`
2. **Check scope** - Ensure tools are registered for chat scope
3. **Review logs** - Look for tool scanning errors

### Performance Issues

1. **Reduce history** - Lower `MaxHistoryMessages` in settings
2. **Optimize context** - Only attach necessary content
3. **Use specific requests** - Avoid overly broad questions

## Best Practices Summary

### Do's ✅
- Set context when working with specific content
- Use clear, specific requests
- Review AI suggestions before applying
- Load relevant tool packs for your task
- Organize attached files by priority
- Use custom instructions for your brand

### Don'ts ❌
- Don't attach unnecessary content
- Don't blindly accept all suggestions
- Don't forget to verify before publishing
- Don't ignore error messages
- Don't overload with too many attachments

## Example Workflows

### Workflow 1: Creating a New Product Page

```
You: "Create a new product page for 'Eco-Friendly Water Bottle'"
AI: [Loads Create tools] "I'll create a new product page. What parent page?"
You: "Under Products"
AI: [Creates page] "Created. Would you like me to add content?"
You: "Yes, generate SEO title and description"
AI: [Loads Update tools] "Here are suggestions: ..."
You: "Apply those and publish"
AI: [Updates and publishes] "Done! Page is live."
```

### Workflow 2: Translating Content

```
You: "Translate this page to Spanish and German"
AI: [Checks context] "I'll translate the current page. Which properties?"
You: "Heading, preamble, and main body"
AI: [Loads translation tools] "Translating... Done!"
You: "Publish the Spanish version"
AI: [Publishes] "Spanish version published."
```

### Workflow 3: SEO Optimization

```
You: "Optimize SEO for all product pages"
AI: [Loads Work Content tools] "Found 25 product pages. Should I analyze?"
You: "Yes"
AI: [Analyzes] "Here are recommendations: ..."
You: "Apply to pages with missing descriptions"
AI: [Updates 12 pages] "Updated 12 pages. Review before publishing?"
You: "Show me page 1"
AI: [Displays content] "Here's the first page: ..."
```

## Next Steps

- **Explore Tools** - Learn about [AI Tools and Function Calling](Tools.md)
- **Customize Instructions** - Create project-specific guidelines
- **Configure Providers** - Set up Azure, Gemini, or custom AI
- **Learn Shortcuts** - Speed up common tasks with shortcuts
- **Watch Videos** - [Video tutorials](https://aiassistant.optimizely.blog/en/videos/)

## Additional Resources

- [Main Documentation](README.md)
- [AI Tools Reference](Tools.md)
- [Configuration Guide](configuration.md)
- [User Manual](user-manual.md)
- [FAQ](faq.md)
- [GitHub Discussions](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)
- [Official Blog](https://aiassistant.optimizely.blog)

---

**Need Help?**
- [Submit feedback from chat](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)
- [Report issues](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/issues)
- [Watch video guides](https://aiassistant.optimizely.blog/en/videos/)
