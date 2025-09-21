# Setup tools for AI

Tools are custom functions you expose from your Optimizely CMS PaaS code that the AI-Assistant can call when answering a question or performing an editor task.

Instead of only generating text, the AI can fetch real data (for example page content, product information, or external content), process it, and return accurate, context-aware results directly in the editor.

## Tools is available from version 2.0 of Epicweb AI Assistant 

Download the built in tools, download nuget package **Epicweb.Optimizely.AIAssistant.Tools**

Register the tools =>  
```csharp
  services
     // Register MCP tool types
     .RegisterMcpToolType(typeof(BuiltinTools))

     using Epicweb.Optimizely.AIAssistant.Tools;
```

### Built-in Tools Overview

The assistant comes with several default tools that let AI retrieve and work with content directly from Optimizely CMS:

- GetContentById – fetch content (or a specific property, e.g. MainBody, Preamble, SEO title) by content ID.
- GetThisContent – fetch all properties of the current page or block (used for tasks like “summarize this page” or “create an SEO title”).
- GetChildContent – fetch children of a page or block (useful for “list all child titles” or “summarize child pages”).
- GetContentByName – search content by name or partial match across all page types.

👉 In short: these tools allow the AI to look up content and structure from the CMS, so it can summarize, generate SEO metadata, build lists, or answer context-aware editorial queries.

## How to implement your Custom Tools

Example tool, fetch product data from Optimizely Commerce

```csharp
using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class ProductInfoTools
{
    [McpServerTool(Name = "GetProductDescription"),
 Description("Get detailed product information based on product number")]
    public static async Task<ProductInfo> GetProductDescriptionAsync(
    [Description("The product number to get information for, e.g. 'P12345', 'B78901'")] string productNumber)
    {
        // implement your logic to fetch product details here

        return new ProductInfo
        {
            ...
        };
    }
    // Product information POCO class
    public class ProductInfo
    {
        public string ProductNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public string Category { get; set; } = string.Empty;
        public Dictionary<string, string> Specifications { get; set; } = new Dictionary<string, string>();
    }
}     
```

Now register the tools in startup: 

```csharp
  services
     // Register MCP tool types
     .RegisterMcpToolType(typeof(ProductInfoTools))
```

**Thats it**

Now you can use prompts like "Create a sales pitch of Product P-5555 and link to the product" or "Create a table of the differences between products BS8877 and L8877"

## Demo video


https://github.com/user-attachments/assets/726e2cb3-11fe-4ef7-8bf9-fafa22a9a1c1


