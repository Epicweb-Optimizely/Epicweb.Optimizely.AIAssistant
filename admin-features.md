# Administration Features Guide

The AI Assistant for Optimizely CMS includes powerful administration tools that help manage AI-assisted workflows across your organization. This guide covers the three core administration features: **Instructions**, **Assistants**, and **Statistics**.

## Table of Contents

- [Administration Overview](#administration-overview)
  - [Accessing Administration](#accessing-administration)
  - [Granting SEO Manager access](#granting-seo-manager-access)
- [Instructions](#instructions)
- [Assistants](#assistants)
- [Statistics](#statistics)
- [Related Resources](#related-resources)

---

## Administration Overview

The Administration area provides a centralized place to manage all AI Assistant features in Optimizely CMS. Administrators can quickly access and configure:

- **Instructions** - Create and manage reusable AI guidance
- **Assistants** - Manage specialized AI experts for different tasks
- **Statistics** - Monitor AI usage, token consumption, and estimated costs

### Accessing Administration

Add the user to `AIEditors` or `WebAdmins` in Optimizely CMS or in the identity provider that supplies the CMS roles. The user can then sign in and open **AI Assistant** from the CMS menu. The **Instructions** and **Assistants** menu items appear when `EnableChat` is enabled.

`services.AddAIAssistant()` registers `AIAssistantAdminAccess` for the administration overview, Instructions, Assistants, Statistics, and the associated admin APIs. The built-in policy is equivalent to this registration.

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("AIAssistantAdminAccess", policy =>
        policy.RequireAuthenticatedUser()
              .RequireRole("AIEditors", "WebAdmins"));
});
```

📺 **Video Overview**: [Watch the AI Assistants and Quick Instructions demo](https://aiassistant.optimizely.blog/en/videos/)

### Granting SEO Manager access

Set `EnableSeoManager` to `true` to enable SEO Manager. Users with admin access can use it. To grant SEO Manager access to additional CMS roles, list them in `Epicweb:AIAssistant:AIAssistantSEOManager` in `appsettings.json`. This example grants access to users in `WebEditors`.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "EnableSeoManager": true,
      "AIAssistantSEOManager": ["WebEditors"]
    }
  }
}
```

Users who qualify through the configured SEO roles alone see only **SEO Manager** in the **AI Assistant** menu. The `AIAssistantSeoManagerAccess` policy applies the same requirements to the SEO Manager page and API. If the role list is omitted or empty, SEO Manager is available to admin users only. See [SEO Manager content permissions](seo-manager.md#content-permissions) for the CMS rights each action requires.

---

## Instructions

**Instructions** are reusable prompts that help editors complete common tasks faster and more consistently.

### What Are Instructions?

Instructions are pre-written prompts that editors can use repeatedly without having to type the same request each time. They ensure consistency in the content creation process and help maintain quality standards across the organization.

### How to Use Instructions (Editor View)

1. Open the **AI Chat** in Optimizely CMS
2. Type `/` in the message input field to open the Instructions list
3. Select the desired Instruction
4. The prepared prompt is added to the input field
5. Send the message or modify the prompt as needed

### Common Use Cases

Instructions are ideal for:

- **Content Improvement** - Enhance readability, structure, and quality
- **SEO Optimization** - Generate titles, meta descriptions, and keywords
- **Accessibility** - Check WCAG compliance and improve accessibility
- **Language Comparison** - Compare different language versions
- **Writing Guidelines** - Enforce brand voice and style guidelines
- **Formatting** - Convert text to specific formats (HTML, bullet points, etc.)
- **Localization** - Prepare content for translation

### Managing Instructions (Administrator View)

#### Creating an Instruction

1. Navigate to **AI Assistant** → **Instructions**
2. Click **Add New Instruction** button
3. Fill in the following fields:
   - **Name** - Short, descriptive name (e.g., "Improve SEO Title")
   - **Short Description** describes the purpose.
   - **Instruction** contains the reusable prompt.
   - **Type** offers **Organization** and **Personal**.
   - **Area** offers **All**, **Chat**, and **Field**. The chat picker lists instructions set to **All** or **Chat**.

4. Click **Save Instruction**.

#### Editing an Instruction

1. Navigate to **AI Assistant** → **Instructions**
2. Find the instruction in the list
3. Click the **Edit** button
4. Modify the fields as needed
5. Click **Save**

#### Deleting an Instruction

1. Navigate to **AI Assistant** → **Instructions**
2. Find the instruction in the list
3. Click the **Delete** button
4. Confirm the deletion

### Sharing Instructions

Instructions can be:

The chat picker shows **Personal** instructions to their owner and **Organization** instructions to chat users. The administration list shows all records to users with access to the administration area.

### Example Instructions

**SEO Title Generator**
```
Generate an optimized SEO title (50-60 characters) for the following content. 
The title should be compelling, include the main keyword, and encourage clicks.
```

**Content Accessibility Check**
```
Review this content for WCAG 2.1 AA compliance. Check for:
- Proper heading hierarchy
- Alternative text for images
- Color contrast issues
- Keyboard navigation considerations
- Semantic HTML usage
Provide a detailed report with recommendations.
```

**Brand Voice Consistency**
```
Review this content for brand voice consistency. Our brand voice is:
[Define your brand voice characteristics here]

Rewrite the content to align with this voice while preserving the core message.
```

---

## Assistants

**Assistants** are specialized AI experts that combine domain knowledge with custom instructions to help editors with specific tasks.

### What Are Assistants?

Assistants are pre-configured AI personalities, each with:

- A unique name and description
- Custom instructions tailored to specific tasks
- Their own expertise in particular domains
- Optional organization-wide or personal availability

When editors start a chat, they can select an appropriate Assistant that understands their specific task requirements.

### How to Use Assistants (Editor View)

1. Open **AI Chat** in Optimizely CMS
2. Select the desired Assistant from the list when starting a new chat
3. The Assistant appears with its name and description
4. The assistant starts with its configured instructions and the current CMS item, when available
5. Interact with the Assistant - it will apply its specialized knowledge

### Common Use Cases

Assistants are ideal for:

- **SEO Expert** - Optimize titles, descriptions, and content for search engines
- **Translation Reviewer** - Ensure translation quality and consistency
- **Content Quality Checker** - Review content for tone, clarity, and impact
- **Accessibility Specialist** - Ensure WCAG compliance
- **Brand Guardian** - Enforce brand guidelines and voice consistency
- **Commerce Specialist** - Assist with product descriptions and e-commerce content
- **Localization Expert** - Prepare content for different markets and languages

### Managing Assistants (Administrator View)

#### Creating an Assistant

1. Navigate to **AI Assistant** → **Assistants**
2. Click **Add New Assistant** button
3. Fill in the following fields:
   - **Name** - The assistant's name (e.g., "SEO Expert")
   - **Short Description** describes the assistant's purpose.
   - **Instructions** contains the prompt that defines the assistant's behavior.
   - **Type** controls which chat users see it. Choose **Organization** or **Personal**.

4. Click **Save Assistant**. Drag rows in the assistants list to change their order.

#### Editing an Assistant

1. Navigate to **AI Assistant** → **Assistants**
2. Find the assistant in the list
3. Click the **Edit** button
4. Modify the fields as needed
5. Click **Save**

#### Deleting an Assistant

1. Navigate to **AI Assistant** → **Assistants**
2. Find the assistant in the list
3. Click the **Delete** button
4. Confirm the deletion

### Example Assistants

**SEO Optimization Assistant**
```
You are an expert SEO specialist. Your role is to help content editors optimize 
their content for search engines. You should:
- Review titles and meta descriptions (60-160 characters)
- Suggest target keywords and their usage
- Check heading hierarchy and structure
- Recommend internal linking opportunities
- Provide readability suggestions
- Analyze content length and depth

Always provide actionable recommendations with specific examples.
```

**Content Quality Reviewer**
```
You are a content quality expert. Your role is to review content for:
- Clarity and readability
- Tone and voice consistency
- Completeness and relevance
- Engagement and call-to-action effectiveness
- Audience appropriateness

Provide constructive feedback with specific examples and improvement suggestions.
Maintain a supportive and encouraging tone.
```

**WCAG Accessibility Specialist**
```
You are a WCAG 2.1 AA accessibility expert. Review all content for compliance:
- Proper semantic HTML and heading hierarchy
- Text alternatives for images
- Color contrast ratios (minimum 4.5:1 for normal text)
- Keyboard navigation and focus management
- Form labels and error messages
- Video captions and transcripts

Provide detailed recommendations for any accessibility issues found.
```

### Benefits of Using Assistants

- **Consistency** - Different editors get consistent guidance
- **Efficiency** - Pre-configured expertise saves setup time
- **Training** - New editors learn best practices from the assistant
- **Quality** - Specialized focus improves content outcomes
- **Scalability** - Share expertise across the organization

---

## Statistics

**Statistics** provides comprehensive insights into how AI Assistant is being used across your organization.

### What Statistics Track

The Statistics view monitors:

- **Total AI Calls** - Number of AI requests made
- **Token Usage** - Input and output tokens consumed
- **Cost Estimation** - Estimated AI service costs based on token prices
- **Usage Trends** - Daily counts within the selected date range
- **Source Breakdown** - Usage by reported source
- **Session Details** - Session IDs and their usage totals

### Accessing Statistics

1. Navigate to **AI Assistant** → **Statistics**
2. View the overview dashboard with key metrics
3. Use filters to narrow down data

### Filtering Statistics

You can filter statistics by:

- **Date Range** - View usage for specific time periods
- **Type** offers **Total**, **Daily**, **Session**, and **Source**.
- **Source** filters the returned sources when **Type** is set to **Source**.

### Understanding Metrics

#### AI Calls
The total number of times editors have used AI Assistant features.

#### Token Usage
- **Input Tokens** - Tokens used for content sent to the AI model
- **Output Tokens** - Tokens used for AI-generated responses
- **Total Tokens** - Sum of input and output tokens

#### Cost Estimation
Estimated cost based on:
- Current token prices for your AI model
- Total tokens consumed
- Configured currency

### Cost Settings

Use **Cost Settings** on the **Statistics** page to enter input and output prices per million tokens and select a currency. The page calculates the estimated cost for the displayed data and saves the values in the current browser.

### Use Cases for Statistics

- **Budget Management** - Track AI service costs and predict spending
- **Adoption Monitoring** - Understand feature usage across teams
- **ROI Analysis** - Estimate time and cost savings
- **Performance Analysis** - Identify peak usage times and patterns
- **User Training** - Data-driven decisions for team enablement
- **Optimization** - Find opportunities to improve efficiency

### Example Reports

**Monthly Cost Overview**
- Review total token consumption for the month
- Compare against budget
- Identify cost trends

**Feature Usage Breakdown**
- See which features (chat, fields, images) consume the most tokens
- Identify high-value features
- Plan feature rollouts and training

**Session Usage Analysis**
- Check how many calls and tokens each reported session used.
- Look for sessions with unusually high token usage.

---

## Related Resources

### Video Guides

📺 **Main Video Portal**: [Watch comprehensive video guides and tutorials](https://aiassistant.optimizely.blog/en/videos/)

- AI Assistants and Quick Instructions Overview
- Step-by-step guides for creating Instructions
- Assistant configuration tutorials
- Statistics and cost tracking guides

### Blog Articles

📖 **Detailed Feature Coverage**: 

- [News in AI Assistant for CMS: Assistants, Instructions and Statistics](https://optimizely.blog/en/2026/08/assistants-instructions-and-statistics/) - Comprehensive overview with screenshots

### Related Documentation

- [AI Chat - Getting Started](chat-instructions.md) - Learn about the chat interface
- [Shortcuts Guide](promptshortcuts.md) - Pre-defined shortcuts and prompts
- [AI Tools - Function Calling and MCP](tools.md) - Custom tool integration
- [Configuration](configuration.md) - Configure AI providers and settings

### Best Practices

#### For Instructions

✅ **DO:**
- Use clear, descriptive names
- Write specific, detailed prompts
- Test instructions before sharing organization-wide
- Document expected inputs and outputs
- Review and update instructions regularly

❌ **DON'T:**
- Use overly generic names
- Create duplicate instructions
- Leave instructions unmaintained
- Make instructions too long or complex

#### For Assistants

✅ **DO:**
- Define a clear specialty for each assistant
- Include the assistant's role in the instruction text
- Set appropriate availability (personal vs. organization-wide)
- Test thoroughly before organization rollout
- Gather user feedback regularly

❌ **DON'T:**
- Create redundant assistants
- Make assistants with conflicting instructions
- Overwhelm editors with too many assistants
- Neglect to update assistant instructions

#### For Statistics

✅ **DO:**
- Review statistics regularly to track trends
- Set up cost budgets based on historical data
- Use data to inform training and optimization
- Share insights with stakeholders
- Monitor for unexpected usage spikes

❌ **DON'T:**
- Ignore cost escalation trends
- Make decisions without understanding your data
- Set unrealistic token price estimates
- Neglect security and compliance tracking

---

## Summary

The Administration features—**Instructions**, **Assistants**, and **Statistics**—work together to:

1. **Improve Quality** - Instructions and Assistants ensure consistent, high-quality output
2. **Increase Efficiency** - Pre-configured expertise saves editors time
3. **Manage Costs** - Statistics help track and control AI service spending
4. **Support Growth** - Administration tools scale expertise across the organization

By leveraging these features, organizations can unlock the full potential of AI Assistant while maintaining quality, consistency, and cost control.

For more information, visit [https://aiassistant.optimizely.blog](https://aiassistant.optimizely.blog)
