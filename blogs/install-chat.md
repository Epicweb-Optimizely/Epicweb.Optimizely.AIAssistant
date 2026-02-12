# How to Install and Configure AI Chat for Optimizely CMS - Complete Guide

**Supercharge your content editing experience with conversational AI assistance directly in Optimizely CMS**

---

## What is AI Chat?

AI Chat is a revolutionary feature introduced in version 3.0 of Epicweb.Optimizely.AIAssistant that brings conversational AI directly into your Optimizely CMS editing experience. Instead of manually filling fields or guessing at optimal content, you can now have natural conversations with AI while working with pages, blocks, and media.

Think of it as having an expert content strategist, SEO specialist, and technical writer all rolled into one—available 24/7, right in your CMS toolbar.

<img width="2836" height="1160" alt="image" src="https://github.com/user-attachments/assets/484869d3-adcf-49b7-b6eb-48bb6a6d6d6d" />


### The Power of Conversational Content Management

Instead of this traditional workflow:
```
1. Open page editor
2. Think about SEO title
3. Write title
4. Check character count
5. Revise
6. Repeat for meta description
7. Manually check accessibility
8. Review content structure
```

You can now do this:
```
You: "Analyze this page's SEO and suggest improvements"
AI: [Analyzes current page, identifies 3 issues]
You: "Apply those suggestions"
AI: [Updates SEO title, meta description, and heading structure] "Done! Review the changes?"
```

---

## Key Benefits

### 🚀 Boost Productivity

- **10x faster content creation** - Generate SEO-optimized content in seconds
- **Automated workflows** - Let AI handle repetitive tasks
- **Smart suggestions** - Get intelligent recommendations based on your content
- **Instant operations** - Update content through natural conversation

### 🎯 Improve Content Quality

- **SEO optimization** - AI suggests search-friendly titles, descriptions, and structure
- **Consistency** - Maintain brand voice across all content
- **Accessibility** - Automatic alt text and WCAG compliance checks
- **Multi-language** - Seamless translation with context awareness

### 💡 Intelligent Assistance

- **Context-aware** - Understands what you're working on
- **Learning capabilities** - Adapts to your content guidelines
- **Real-time help** - Get answers without leaving the CMS
- **Content analysis** - Instant feedback on readability, tone, and structure

### 🛠️ Powerful Tool Integration

- **Content operations** - Create, read, update, and publish through conversation
- **Image handling** - Generate alt text and analyze images
- **External systems** - Integrate with APIs, databases, and services
- **Custom tools** - Extend with your own business logic

### 👥 Team Empowerment

- **Lower barrier to entry** - New editors productive from day one
- **Best practices built-in** - AI guides editors toward quality content
- **Role-based access** - Control who can use AI features (optional)
- **Audit trail** - Track all AI-assisted changes

---

## Prerequisites

Before installing AI Chat, ensure you have:

✅ **Optimizely CMS 12.18+** or **Optimizely Commerce 14+**  
✅ **Epicweb.Optimizely.AIAssistant 3.0+** installed  
✅ **OpenAI API key** or compatible AI provider (Azure OpenAI, Google Gemini)  
✅ **Developer access** to your solution's source code  
✅ **.NET 6.0+** (or compatible with your CMS version)

---

## Step-by-Step Installation

### Step 1: Install Base Package

If you haven't already installed the AI Assistant, start here:

**Via NuGet Package Manager Console:**
```powershell
Install-Package Epicweb.Optimizely.AIAssistant
```

**Or via .NET CLI:**
```bash
dotnet add package Epicweb.Optimizely.AIAssistant
```

**NuGet URL:**  
https://nuget.optimizely.com/package/?id=Epicweb.Optimizely.AIAssistant

---

### Step 2: Install Built-in Tools Package

AI Chat requires tools to function. The built-in tools package provides essential functionality:

**Via NuGet Package Manager Console:**
```powershell
Install-Package Epicweb.Optimizely.AIAssistant.Tools
```

**Or via .NET CLI:**
```bash
dotnet add package Epicweb.Optimizely.AIAssistant.Tools
```

**NuGet URL:**  
https://nuget.optimizely.com/package/?id=Epicweb.Optimizely.AIAssistant.Tools

💡 **Why are tools important?** Tools enable AI to perform real operations—fetching content, updating properties, publishing pages—instead of just generating text. Without tools, the AI can only suggest changes; with tools, it can actually execute them.

---

### Step 3: Configure Services in Startup.cs

Open your `Startup.cs` file and register the AI Assistant with all necessary tool packs:

```csharp
using Epicweb.Optimizely.AIAssistant;
using Epicweb.Optimizely.AIAssistant.Tools;
using Epicweb.Optimizely.AIAssistant.Hub;

public void ConfigureServices(IServiceCollection services)
{
    // ... your existing services ...
    
    services
        .AddAIAssistant()
        // Register built-in tool types for AI Chat functionality
        .RegisterMcpToolType(typeof(BuiltinTools))              // Core helper tools
        .RegisterMcpToolType(typeof(BuiltinChatTools))          // Content reading
        .RegisterMcpToolType(typeof(BuiltinPublishChatTools))   // Publishing operations
        .RegisterMcpToolType(typeof(BuiltinUpdateChatTools))    // Content updates
        .RegisterMcpToolType(typeof(BuiltinChatImageTools))     // Image analysis
        .RegisterMcpToolType(typeof(BuiltinChatCreateImageTools)); // Image generation
        
    // ... other registrations ...
}
```

**Tool Pack Breakdown:**

| Tool Pack | Purpose | Example Operations |
|-----------|---------|-------------------|
| `BuiltinTools` | Core helper utilities | Date formatting, string manipulation |
| `BuiltinChatTools` | Content reading | Get page content, search, list children |
| `BuiltinPublishChatTools` | Publishing | Publish, unpublish, schedule |
| `BuiltinUpdateChatTools` | Content editing | Update properties, create drafts |
| `BuiltinChatImageTools` | Image operations | Analyze images, generate alt text |
| `BuiltinChatCreateImageTools` | Image creation | Intruction how to create images |

---

### Step 4: Register SignalR Hub

AI Chat uses SignalR for real-time communication. Add the hub endpoint in your `Configure` method:

```csharp
using Epicweb.Optimizely.AIAssistant.Hub;

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ... existing middleware ...
    
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapContent();
        endpoints.MapAIAssistantHub(); // ⚠️ Required for AI Chat
    });
}
```

⚠️ **Important:** Without `MapAIAssistantHub()`, the chat window will fail to connect and display errors.

---

### Step 5: Configure in appsettings.json

Add AI Chat configuration to your `appsettings.json`:

```json
{
  "Epicweb": {
    "AIAssistant": {
      "ApiKey": "sk-proj-your-openai-api-key-here", //optional on localhost
      "Provider": "OpenAI", //optional on localhost
      "Model": "gpt-5.2",
      "EnableChat": true,
      "ChatRoles": [ //optional
        "AIEditors",
        "WebEditors", 
        "WebAdmins",
        "CmsAdmins",
        "CmsEditors"
      ],
      "MaxToolIterations": 8 //optional
    }
  }
}
```

**Configuration Options Explained:**

| Setting | Type | Description | Default |
|---------|------|-------------|---------|
| `ApiKey` | string | Your OpenAI API key from https://platform.openai.com/account/api-keys | **Required** |
| `Provider` | string | AI provider: `OpenAI`, `AzureOpenAI`, `Gemini` | `OpenAI` |
| `Model` | string | AI model to use (see recommendations below) | `gpt-5.2` |
| `EnableChat` | bool | Enables the AI Chat window in CMS toolbar | `false` |
| `ChatRoles` | string[] | User roles that can access AI Chat (**optional**, if not specified all authenticated users can access) | All users |
| `MaxToolIterations` | int | **optional** Prevents infinite tool calling loops | `8` |

#### Getting Your API Key

1. Go to https://platform.openai.com/account/api-keys
2. Sign up or log in
3. Click "Create new secret key"
4. Copy the key (starts with `sk-`)
5. Add to `appsettings.json`

**🔒 Security Tip:** Never commit API keys to source control. Use:
- Azure Key Vault
- Environment variables
- User secrets (`dotnet user-secrets set "Epicweb:AIAssistant:ApiKey" "sk-..."`)

---

### Step 6: Choose the Right AI Model

⚠️ **Critical:** AI Chat requires advanced models with robust tool calling capabilities.

#### ✅ Recommended Models

**OpenAI:**
- `gpt-5.2` - Latest, best overall performance (recommended)
- `gpt-5.1` - Excellent tool coordination and reasoning
- `gpt-5` - Solid performance with tool calling
- `gpt-5.5-mini` - Budget-friendly option with good tool support

**Azure OpenAI:**
- `gpt-5.2` - Same as OpenAI, but hosted in Azure
- `gpt-5.1` - Robust enterprise option

**Google Gemini:**
- `gemini-2.5-pro` - Latest stable version (recommended)
- `gemini-2.5-flash` - Faster, cost-effective option

#### ❌ Models NOT Compatible with AI Chat

- ❌ `gpt-4o`, `gpt-4-turbo`, `gpt-4` - Older generation, incompatible tool calling
- ❌ `gpt-3.5-turbo` - No advanced tool coordination
- ❌ `gemini-1.5-pro`, `gemini-1.5-flash` - Older Gemini versions not supported

**Why model choice matters:** AI Chat uses advanced "function calling" to execute multiple tools in complex sequences. Only GPT-5+ and Gemini 2.5+ models have the necessary capabilities to:
- Chain multiple tool calls together
- Handle errors and retry with different approaches
- Maintain context across long conversations
- Understand when to use specific tools

---

### Step 7: Configure Role-Based Access (Optional)

By default, all authenticated CMS users can access AI Chat. To restrict access to specific roles:

**Example: Restrict to admins only**
```json
"ChatRoles": ["CmsAdmins", "WebAdmins"]
```

**Example: Allow specific editor roles**
```json
"ChatRoles": ["CmsEditors", "WebEditors", "AIEditors", "CmsAdmins"]
```

**Example: Custom roles**
```json
"ChatRoles": ["ContentCreators", "SEOSpecialists"]
```

**To allow all users:** Simply omit the `ChatRoles` setting or set it to an empty array:
```json
"ChatRoles": []
```

Make sure users are assigned these roles in Optimizely's admin interface under:  
**Admin → Administer Groups**

---

### Step 8: Verify Installation

After completing the setup:

1. **Build your solution**
   ```bash
   dotnet build
   ```

2. **Run the application**
   ```bash
   dotnet run
   ```

3. **Log in to Optimizely CMS**

4. **Look for the AI Chat button** in the CMS toolbar (typically top-right)

5. **Click to open the chat window**

6. **Test with a simple command:**
   ```
   "What tools are available?"
   ```

If you see a list of tools, congratulations! AI Chat is working. 🎉

---

## Advanced Configuration

### Custom Chat Instructions

Customize AI behavior for your organization:

#### 1. Create Instruction Files

Create a folder structure in your project:

```
YourProject/
  Instructions/
    chat/
      global.md          # Overall behavior
      general.md         # Content guidelines
      context.md         # Context handling rules
    ContentType/
      ProductPage.md     # Product-specific rules
      NewsPage.md        # News-specific rules
      ArticlePage.md     # Article-specific rules
    Field/
      MainBody.md        # Field-specific guidance
      SEODescription.md  # SEO field rules
```

#### 2. Example: Global Instructions

**Instructions/chat/global.md**
```markdown
# AI Chat Instructions for Acme Corporation

You are assisting content editors at Acme Corporation, a leading provider of sustainable products.

## Brand Voice
- Professional yet friendly and approachable
- Emphasize sustainability and environmental responsibility
- Use "solutions" instead of "products"
- Highlight our commitment to quality and customer service

## Content Standards
- All product pages must include eco-certifications
- SEO descriptions must be exactly 155 characters
- Always suggest WCAG 2.1 AA compliant content
- Include relevant keywords naturally (no keyword stuffing)

## Response Style
- Be concise and actionable
- Provide examples when helpful
- Always ask for confirmation before publishing
- Explain your reasoning for suggestions

## Tone Guidelines
- Avoid jargon unless industry-standard
- Use active voice
- Write in second person when addressing customers
- Maintain consistent terminology across all content
```

#### 3. Example: Content Type Instructions (Template-Specific)

**Instructions/ContentType/ProductPage.md**
```markdown
# Product Page Instructions

When working with Product pages, follow these guidelines:

## Required Content Structure
- **Hero Image**: Product image with white background (1200x1200px)
- **Product Name**: Clear, descriptive, max 60 characters
- **SKU**: Always visible, format: PRD-XXXXX
- **Price**: Include currency symbol, no decimals for whole numbers
- **Description**: 2-3 paragraphs highlighting key benefits
- **Specifications**: Bullet list format
- **Sustainability Badge**: Required for all products

## Recommended Blocks
When editors ask for content suggestions, recommend these blocks:
- **Product Features Block** - Highlight 3-5 key features with icons
- **Technical Specifications Block** - Detailed product specs in table format
- **Customer Reviews Block** - Display ratings and testimonials
- **Related Products Block** - Show 4 similar or complementary products
- **Sustainability Info Block** - Eco-certifications and environmental impact

## SEO Requirements
- Meta Title: "{Product Name} | {Category} | Acme Corporation" (max 60 chars)
- Meta Description: Include product benefit, material, and CTA (exactly 155 chars)
- Image Alt Text: "{Product Name} - {Key Feature}" format

## Content Automation
When creating product pages:
1. Auto-generate SKU if not provided (PRD-{5-digit-number})
2. Suggest Product Features Block placement after description
3. Add Related Products Block at bottom
4. Include Sustainability Info Block if eco-certified
5. Generate schema.org Product markup
```

**Instructions/ContentType/NewsPage.md**
```markdown
# News Article Instructions

When working with News pages:

## Content Structure
- **Headline**: Compelling, clear, 60-80 characters
- **Preamble**: 1-2 sentences summarizing the story (150-160 chars)
- **Publication Date**: Auto-set to current date
- **Author**: Required field
- **Featured Image**: 1200x630px, with descriptive alt text
- **Body**: 300-800 words, structured with H2/H3 headings

## Recommended Blocks
- **Pull Quote Block** - Highlight key quotes from the article
- **Related Articles Block** - 3 contextually similar news items
- **Author Bio Block** - Short author information with photo
- **Share Buttons Block** - Social media sharing options
- **Newsletter CTA Block** - Encourage newsletter signup

## Style Guidelines
- Use active voice and present tense for current events
- Include quotes from relevant stakeholders
- Link to related internal content (2-3 links)
- Add relevant tags for categorization

## SEO & Social
- Meta Title: "{Headline} | Acme News" (max 60 chars)
- Meta Description: Use preamble (150-160 chars)
- Open Graph Image: Use featured image
- Twitter Card: Summary with large image
```

This template-specific guidance helps editors:
- **Find the right blocks** - AI suggests contextually appropriate blocks for each content type
- **Maintain consistency** - Templates ensure brand standards are followed
- **Speed up workflow** - Pre-defined structures reduce decision fatigue
- **Improve quality** - Built-in best practices guide content creation

#### 4. Mark as Embedded Resources

Edit your `.csproj` file:

```xml
<ItemGroup>
  <EmbeddedResource Include="Instructions\**\*.md" />
</ItemGroup>
```

#### 5. Register in Startup.cs

```csharp
using System.Reflection;

services.AddAIAssistant()
       .AddEmbeddedInstructionStore(
           Assembly.GetExecutingAssembly(),
           priority: 20  // Higher priority overrides defaults
       );
```

**Priority System:**
- Built-in instructions: Priority 10
- Your custom instructions: Priority 20+
- Higher numbers override lower numbers

---

### Creating Custom Tools

Extend AI Chat with your own business logic:

#### Example: Product Information Tool

```csharp
using Epicweb.Optimizely.AIAssistant.Models;
using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class ProductTools
{
    [McpServerTool(Name = "GetProductInventory")]
    [Description("Get real-time inventory status for a product")]
    public static async Task<InventoryInfo> GetProductInventory(
        [Description("Product SKU or ID")] 
        string productId)
    {
        // Your business logic here
        var inventory = await YourInventoryService.GetStockAsync(productId);
        
        return new InventoryInfo
        {
            ProductId = productId,
            QuantityAvailable = inventory.Quantity,
            ReorderLevel = inventory.ReorderLevel,
            NextRestockDate = inventory.NextRestock,
            Status = inventory.Quantity > 0 ? "In Stock" : "Out of Stock"
        };
    }
}

public class InventoryInfo
{
    public string ProductId { get; set; }
    public int QuantityAvailable { get; set; }
    public int ReorderLevel { get; set; }
    public DateTime? NextRestockDate { get; set; }
    public string Status { get; set; }
}
```

**Register your custom tool:**

```csharp
services.AddAIAssistant()
       .RegisterMcpToolType(typeof(ProductTools));
```

**Use in chat:**
```
You: "Check inventory for product P-12345"
AI: [Calls GetProductInventory("P-12345")]
AI: "Product P-12345: In Stock, 47 units available. Next restock: Jan 15, 2024"
```

Learn more: [Complete Tools Guide](../tools.md)

---

## Usage Examples

### Example 1: SEO Optimization for Current Page

```
You: "Analyze this page's SEO and suggest improvements"

AI: [Analyzes current page]
AI: "I found 3 SEO issues on this page:
     
     1. Meta title is 72 characters (should be max 60)
     2. Meta description missing
     3. No H1 heading found in main content
     
     Would you like me to fix these?"

You: "Yes, please"

AI: [Updates the page]
AI: "✅ SEO improvements applied:
     
     - Shortened meta title to: 'Eco-Friendly Water Bottles | Sustainable Drinkware'
     - Added meta description: 'Discover our range of sustainable water bottles...' (154 chars)
     - Restructured main heading as H1
     
     Would you like to publish these changes?"
```

---

### Example 2: Translation Workflow

```
You: "Translate this page to Spanish"

AI: [Analyzes current page content]
AI: "I'll translate the following properties to Spanish:
     - Heading
     - Preamble
     - MainBody
     - SEODescription
     
     Should I proceed?"

You: "Yes, and publish it"

AI: [Translates all properties]
AI: "✅ Spanish translation complete and published:
     
     - Heading: 'Botellas de Agua Ecológicas'
     - All content translated maintaining brand voice
     - SEO metadata translated
     - Page live at /es/botellas-agua-ecologicas"
```

---



## Pro Tips 💡

### Using @ Mentions for Tools

You can directly reference tools in your chat messages using the `@` symbol. This helps you:
- Discover available tools quickly
- Explicitly invoke specific tools
- See tool parameters and descriptions

**How to use @ mentions:**

1. **Type @ in the chat input** - A dropdown appears showing available tools
2. **Select a tool** - The tool name is inserted into your message
3. **Add parameters** - Continue typing your request with context

**Examples:**

```
You: "@GetContentProperties this"
AI: [Returns the content of current page]

You: "@GetContentByName sustainability"
AI: [Searches for pages containing "sustainability"]

You: "@UpdateMultiplePropertiesOnPageOrBlock SEOTitle 'New Optimized Title'"
AI: [Updates the SEO title]
```

### Loading Tool Packs

Instead of waiting for the AI to auto-load tools, you can explicitly load tool packs:

**Syntax:**
```
@LoadToolPack "toolpack-name"
```

**Available Tool Packs:**
- `update` - Content update tools
- `publish` - Publishing and scheduling tools
- `create` - Content creation tools
- `image` - Image analysis and generation tools
- `workcontent` - Content reading and search tools

**Examples:**

```
You: "@LoadToolPack update"
AI: "✅ Update tool pack loaded! You can now:
     - Update content properties
     - Create drafts
     - Modify multiple fields
     
     What would you like to update?"

You: "@LoadToolPack publish"
AI: "✅ Publish tool pack loaded! Available operations:
     - Publish content
     - Unpublish content
     - Schedule publishing
     - Check publish status"

You: "@LoadToolPack create"
AI: "✅ Create tool pack loaded! You can now:
     - Create Language Version
     - Create blocks
     - Set up content structure"
```

**Why load tool packs explicitly?**
- **Faster responses** - Tools are ready immediately
- **See capabilities** - AI lists what you can do with the pack
- **Control context** - Only load what you need for your task
- **Troubleshoot** - Verify tools are working correctly

**Pro workflow example:**

```
You: "@LoadToolPack update"
AI: "✅ Update tool pack loaded!"

You: "Update the SEO title on this page to include our brand name"
AI: [Uses UpdateContentProperty tool]
AI: "✅ SEO title updated to: 'Sustainable Products | Acme Corporation'"

You: "@LoadToolPack publish"
AI: "✅ Publish tool pack loaded!"

You: "Schedule this page to publish tomorrow at 9 AM"
AI: [Uses SchedulePublish tool]
AI: "✅ Scheduled for January 16, 2024 at 9:00 AM"
```

**Tip:** You can load multiple tool packs in one session. The AI will remember which packs are loaded and use the appropriate tools for your requests.

---

## Troubleshooting

### Chat Button Not Visible

**Possible causes:**
- `EnableChat` is `false` in appsettings.json
- User role not in `ChatRoles` array (if roles are configured)
- Hub endpoint not registered

**Solution:**
1. Verify `appsettings.json` has `"EnableChat": true`
2. If using role-based access, check user is in allowed role
3. Confirm `endpoints.MapAIAssistantHub()` is in Startup.cs

---

### Chat Connects But Tools Don't Work

**Possible causes:**
- Tools not registered in Startup.cs
- Using incompatible AI model (must be GPT-5+ or Gemini 2.5+)
- API errors or rate limits

**Solution:**
1. Verify all `RegisterMcpToolType` calls in Startup.cs
2. **Check you're using GPT-5, GPT-5.1, GPT-5.2, GPT-5.5-mini, or Gemini 2.5+**
3. Review application logs for errors
4. Check OpenAI dashboard for API status

---

### "Model not supported" or Tool Calling Fails

**Possible causes:**
- Using older model (GPT-4, GPT-4o, Gemini 1.5)
- Model doesn't support advanced function calling

**Solution:**
1. **Update to GPT-5 or later** (gpt-5, gpt-5.1, gpt-5.2, gpt-5.5-mini)
2. **For Gemini, use 2.5 or later** (gemini-2.5-pro, gemini-2.5-flash)
3. Verify model name is correct in appsettings.json
4. Check provider documentation for model availability

---

### "Tool not found" Errors

**Possible causes:**
- Tool scope mismatch (Field vs Chat)
- Tool not registered for chat scope

**Solution:**
1. Ensure tool has `[ToolScope(ToolScope.Chat)]` attribute
2. Verify tool is in a registered `McpServerToolType` class
3. Check tool name matches exactly (case-sensitive)

---

### Performance Issues

**Possible causes:**
- Too many tools loaded
- Large context attachments
- Slow external API calls in custom tools

**Solution:**
1. Reduce `MaxToolIterations` to 5-6
2. Only attach necessary content to context
4. Use async methods for I/O operations

---

### API Key Errors

**Error:** "Invalid API key" or "Unauthorized"

**Solution:**
1. Verify API key starts with `sk-proj-` (new format) or `sk-`
2. Check key is active at https://platform.openai.com/account/api-keys
3. Ensure no extra spaces in appsettings.json
4. For Azure: Verify endpoint URL and authentication method

---

## Best Practices

### ✅ Do's

- **Start with built-in tools** - They cover 90% of use cases
- **Test with simple commands** - "What's on this page?" before complex operations
- **Review before publishing** - Always check AI suggestions
- **Use specific instructions** - Clear requests get better results
- **Set context** - Attach relevant pages/blocks for accurate results
- **Use template instructions** - Guide AI with content type-specific rules
- **Train your team** - Show editors how to use chat effectively

### ❌ Don'ts

- **Don't blindly publish** - Always review AI-generated content
- **Don't use old models** - Must be GPT-5+ or Gemini 2.5+
- **Don't attach too much** - Keep context focused and relevant
- **Don't skip testing** - Verify changes in staging first
- **Don't ignore errors** - Check logs and fix configuration issues

---

## Cost Optimization

### Tips to Reduce API Costs

1. **Be specific in requests**
   - ❌ "Improve all content on the site"
   - ✅ "Update the meta description on this page"

2. **Cache common operations** in custom tools

3. **Use Azure OpenAI** for enterprise pricing and limits

**Estimated Costs (OpenAI, approximate):**
- Simple request (1-2 tools): $0.01-0.03
- Complex workflow: $0.05-0.15
- Translation task: $0.08-0.20

---

## Next Steps

🎉 **Congratulations!** You've successfully installed and configured AI Chat for Optimizely CMS.

### Continue Your Journey:

1. **Explore and Build Tools**  
   Discover all available tools in the [AI Tools Reference](../tools.md)

2. **Customize Instructions**  
   Create brand-specific and template-specific AI behavior [Instructions Guide](../chat-instructions.md)

3. **Watch Video Tutorials**  
   https://aiassistant.optimizely.blog/en/videos/

4. **Join the Community**  
   https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions

---

## Free Evaluation License

Want to try AI Chat in production without licensing messages?

**Complete the evaluation form:**  
https://aiassistant.optimizely.blog

Get a free evaluation license and explore all features risk-free.

---

## Resources

- 📚 [Complete Documentation](../README.md)
- 🛠️ [AI Tools Guide](../tools.md)
- 💬 [Chat User Manual](../chat-instructions.md)
- ⚙️ [Configuration Reference](../configuration.md)
- 🎥 [Video Tutorials](https://aiassistant.optimizely.blog/en/videos/)
- 💬 [GitHub Discussions](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)
- 🐛 [Report Issues](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/issues)
- 📝 [Official Blog](https://aiassistant.optimizely.blog)

---

## Support

Need help with your AI Chat installation?

- **GitHub Discussions:** Ask questions and share experiences
- **GitHub Issues:** Report bugs and feature requests
- **Blog:** Read articles and case studies
- **Videos:** Watch step-by-step tutorials

---

**Package Maintainer:** [lucgosso](https://github.com/lucgosso)

**License:** [View Terms of Use](../license.md)

---

*Made with ❤️ for the Optimizely community*
