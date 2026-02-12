# AI Tools - Function Calling and MCP

AI Tools (Function Calling / Model Context Protocol) allow the AI Assistant to perform real operations instead of just generating text. They enable the AI to fetch data, update content, perform operations, and connect to external systems.

**Available from version 2.0 of Epicweb.Optimizely.AIAssistant**

## Quick Start

### Installation

Install the built-in tools NuGet package:

```bash
Install-Package Epicweb.Optimizely.AIAssistant.Tools
```

Or via .NET CLI:
```bash
dotnet add package Epicweb.Optimizely.AIAssistant.Tools
```

### Basic Setup

Register the built-in tools in `Startup.cs`:

```csharp
using Epicweb.Optimizely.AIAssistant.Tools;

public void ConfigureServices(IServiceCollection services)
{
    services.AddAIAssistant()
           // Register built-in MCP tool types
           .RegisterMcpToolType(typeof(BuiltinTools))
           .RegisterMcpToolType(typeof(BuiltinChatTools))
           .RegisterMcpToolType(typeof(BuiltinPublishChatTools))
           .RegisterMcpToolType(typeof(BuiltinUpdateChatTools))
           .RegisterMcpToolType(typeof(BuiltinChatImageTools))
           .RegisterMcpToolType(typeof(BuiltinChatCreateImageTools));
}
```

**That's it!** Your AI Assistant can now use tools to interact with your CMS content.

### Quick Example - Custom Product Tool

Create a custom tool to fetch product data from Optimizely Commerce:

```csharp
using Epicweb.Optimizely.AIAssistant.Models;
using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class ProductInfoTools
{
    [McpServerTool(Name = "GetProductDescription")]
    [Description("Get detailed product information based on product number")]
    public static async Task<ProductInfo> GetProductDescriptionAsync(
        [Description("The product number to get information for, e.g. 'P12345', 'B78901'")] 
        string productNumber,
        RequestModel context = null) // Optional: context provides current page, language, property info
    {
        // Implement your logic to fetch product details here
        var product = await YourProductService.GetProductAsync(productNumber);
        
        return new ProductInfo
        {
            ProductNumber = product.Code,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            InStock = product.Inventory.InStock,
            Category = product.Category,
            Specifications = product.Specs
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
        public Dictionary<string, string> Specifications { get; set; } = new();
    }
}
```

Register your custom tool:

```csharp
services.AddAIAssistant()
       .RegisterMcpToolType(typeof(ProductInfoTools));
```

Now you can use prompts like:
- "Create a sales pitch for Product P-5555 and link to the product"
- "Create a comparison table of products BS8877 and L8877"
- "What's the current price of product P-12345?"

## What are AI Tools?

AI Tools are C# methods that you expose to the AI Assistant. When the AI determines it needs specific data or should perform an action, it can call these tools with appropriate parameters.

### Why Use Tools?

**Without Tools:**
```
User: "What's on the About page?"
AI: "I don't have access to your content. Please copy and paste the text."
```

**With Tools:**
```
User: "What's on the About page?"
AI: [Calls GetContentByName("About")] 
AI: "The About page contains: [actual content from CMS]..."
```

### Key Benefits

- **Real Data** - AI works with actual CMS content, not assumptions
- **Accurate Results** - No hallucinations about your content
- **Automated Operations** - AI can update, publish, and manage content
- **Extensible** - Add custom tools for any business logic
- **Secure** - Tools run with proper permissions and validation

## Built-in Tools

The plugin comes with several built-in tool packs. Install the NuGet package:

```bash
Install-Package Epicweb.Optimizely.AIAssistant.Tools
```

### Core Tools Package

Register the built-in tools in `Startup.cs`:

```csharp
using Epicweb.Optimizely.AIAssistant.Tools;

public void ConfigureServices(IServiceCollection services)
{
    services.AddAIAssistant()
           .RegisterMcpToolType(typeof(BuiltinTools))
           .RegisterMcpToolType(typeof(BuiltinChatTools))
           .RegisterMcpToolType(typeof(BuiltinPublishChatTools))
           .RegisterMcpToolType(typeof(BuiltinUpdateChatTools))
           .RegisterMcpToolType(typeof(BuiltinChatImageTools))
           .RegisterMcpToolType(typeof(BuiltinChatCreateImageTools));
}
```

### Built-in Tool Reference

#### BuiltinTools (Helper Tools)

| Tool | Description | Example |
|------|-------------|---------|
| `FormatDate` | Format dates in various formats | "Format today as 'Jan 15, 2024'" |
| `GetCurrentDate` | Get current date/time | "What's today's date?" |
| `StringManipulation` | String operations (uppercase, lowercase, trim) | "Convert to uppercase" |
| `CalculateLength` | Get text or content length | "How long is this text?" |

#### BuiltinChatTools (Content Reading)

Core tools that allow the AI to retrieve and work with content directly from Optimizely CMS:

| Tool | Description | Example |
|------|-------------|---------|
| `GetContentById` | Get content by ID or reference | "Get content 123" |
| `GetContentProperty` | Get specific property value (e.g. MainBody, Preamble, SEO title) | "Get MainBody from page 123" |
| `GetThisContent` | Get current page/block content (all properties) | "Show me this page's properties" |
| `GetChildContent` | Get children of a page (useful for "list all child titles") | "List all child pages" |
| `GetContentByName` | Search content by name or partial match across all page types | "Find page named 'Products'" |
| `SearchContent` | Full-text content search | "Search for 'sustainability'" |
| `GetPageStructure` | Get page tree structure | "Show site structure" |

👉 **In short:** These tools allow the AI to look up content and structure from the CMS, so it can summarize, generate SEO metadata, build lists, or answer context-aware editorial queries.

#### BuiltinPublishChatTools (Publishing)

| Tool | Description | Example |
|------|-------------|---------|
| `PublishContent` | Publish content | "Publish this page" |
| `UnpublishContent` | Unpublish content | "Unpublish the News page" |
| `SchedulePublish` | Schedule publishing | "Schedule for tomorrow 10am" |
| `GetPublishStatus` | Check publish status | "Is this page published?" |

#### BuiltinUpdateChatTools (Content Updates)

| Tool | Description | Example |
|------|-------------|---------|
| `UpdateContentProperty` | Update a property value | "Set heading to 'Welcome'" |
| `UpdateMultipleProperties` | Update several properties | "Update heading and preamble" |
| `CreateDraft` | Create a new version | "Create a draft version" |

#### BuiltinChatImageTools (Image Operations)

| Tool | Description | Example |
|------|-------------|---------|
| `AnalyzeImage` | Analyze image content | "Describe this image" |
| `GenerateAltText` | Generate alt text | "Create alt text for images" |
| `GetImageMetadata` | Get image properties | "Show image dimensions" |

#### BuiltinChatCreateImageTools (Image Generation)

| Tool | Description | Example |
|------|-------------|---------|
| `ImageCreationInstructions` | Image Creation instruction | "Create image of ..." |

## Tool Scopes

Tools can be scoped to specific areas:

```csharp
public enum ToolScope
{
    Fields = 1,    // Available in property editors
    Chat = 2,      // Available in chat window
    All = 3        // Available everywhere (Fields | Chat)
}
```

### Setting Tool Scope

Use the `[ToolScope]` attribute:

```csharp
[McpServerToolType]
public static class MyTools
{
    [McpServerTool(Name = "ChatOnlyTool")]
    [ToolScope(ToolScope.Chat, PackageId = "MyToolPack")]
    [Description("Only available in chat")]
    public static string ChatOnlyTool() => "Hello from chat!";
    
    [McpServerTool(Name = "FieldOnlyTool")]
    [ToolScope(ToolScope.Fields)]
    [Description("Only in property editors")]
    public static string FieldOnlyTool() => "Hello from field!";
}
```

## Creating Custom Tools

### Basic Custom Tool

Create a static class with methods marked with `[McpServerTool]`:

```csharp
using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class ProductTools
{
    [McpServerTool(Name = "GetProductInfo")]
    [Description("Get detailed product information by product number")]
    public static ProductInfo GetProductInfo(
        [Description("Product number (e.g., 'P12345')")] 
        string productNumber)
    {
        // Your logic to fetch product
        var product = ProductRepository.GetByNumber(productNumber);
        
        return new ProductInfo
        {
            ProductNumber = productNumber,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            InStock = product.InStock
        };
    }
}

public class ProductInfo
{
    public string ProductNumber { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool InStock { get; set; }
}
```

### Register Custom Tools

In `Startup.cs`:

```csharp
services.AddAIAssistant()
       .RegisterMcpToolType(typeof(ProductTools));
```

### Using Custom Tools

Once registered, the AI can use them:

```
User: "Create a product comparison for P12345 and P67890"
AI: [Calls GetProductInfo("P12345")]
AI: [Calls GetProductInfo("P67890")]
AI: "Here's a comparison table: ..."
```

## Advanced Tool Patterns

### Async Tools

Tools can be asynchronous:

```csharp
[McpServerTool(Name = "FetchExternalData")]
[Description("Fetch data from external API")]
public static async Task<ExternalData> FetchExternalData(
    [Description("API endpoint")] string endpoint)
{
    using var client = new HttpClient();
    var response = await client.GetAsync(endpoint);
    var data = await response.Content.ReadAsStringAsync();
    
    return JsonSerializer.Deserialize<ExternalData>(data);
}
```

### Tools with Context

Access the current request context:

```csharp
using Epicweb.Optimizely.AIAssistant.Models;

[McpServerTool(Name = "GetCurrentPageInfo")]
[Description("Get information about the current page being edited")]
public static PageInfo GetCurrentPageInfo(RequestModel context = null)
{
    if (context?.ContentId == null)
        return null;
        
    var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
    var content = contentLoader.Get<IContent>(
        new ContentReference(context.ContentId));
        
    return new PageInfo
    {
        Name = content.Name,
        ContentType = content.ContentType.Name,
        Language = context.Lang
    };
}
```

### Complex Parameters

Tools can accept complex objects:

```csharp
[McpServerTool(Name = "BulkUpdatePages")]
[Description("Update multiple pages at once")]
public static BulkUpdateResult BulkUpdatePages(
    [Description("List of page IDs to update")] 
    List<string> pageIds,
    
    [Description("Properties to update")] 
    Dictionary<string, string> properties)
{
    var result = new BulkUpdateResult();
    var contentRepository = ServiceLocator.Current
        .GetInstance<IContentRepository>();
    
    foreach (var pageId in pageIds)
    {
        try
        {
            var contentRef = new ContentReference(pageId);
            var content = contentRepository
                .Get<IContent>(contentRef)
                .CreateWritableClone();
                
            foreach (var prop in properties)
            {
                content.Property[prop.Key].Value = prop.Value;
            }
            
            contentRepository.Save(content, SaveAction.Publish);
            result.SuccessCount++;
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Page {pageId}: {ex.Message}");
        }
    }
    
    return result;
}

public class BulkUpdateResult
{
    public int SuccessCount { get; set; }
    public List<string> Errors { get; set; } = new();
}
```

## Property JSON Converters

Property JSON Converters enable the AI to properly format JSON for complex Optimizely property types like `ContentArea`, `ContentReference`, `XhtmlString`, and custom business types.

### Why Property Converters Matter

When AI tools update content properties, they need to understand the correct JSON format for each property type. Property converters provide:

- **AI Instructions** - Clear guidelines on how to format JSON
- **JSON Schema** - Validation constraints
- **Bidirectional Conversion** - Transform between JSON and CLR types
- **Type Safety** - Ensure data integrity

### Quick Example

```csharp
public class CategoryListJsonConverter : IPropertyJsonConverter
{
    public Type ClrType => typeof(IList<string>);

    public bool CanHandle(PropertyInfo propertyInfo)
    {
        return propertyInfo.Name == "Categories";
    }

    public string GetInstructions(PropertyInfo propertyInfo)
    {
        return @"
**Categories Property Format**

Array of category name strings.

**Example:**
```json
{
  ""Categories"": [""Electronics"", ""Computers""]
}
```

**Rules:**
- Must be an array of strings
- 1-5 items required
- No duplicates
";
    }

    public object? Convert(JsonElement json, PropertyInfo propertyInfo)
    {
        if (json.ValueKind != JsonValueKind.Array)
            return null;

        var categories = new List<string>();
        foreach (var item in json.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.String)
            {
                categories.Add(item.GetString());
            }
        }
        return categories;
    }

    // ... other methods
}
```

### Registration

```csharp
services.AddAIAssistant()
       .RegisterPropertyConverter<CategoryListJsonConverter>();
```

### Learn More

For comprehensive documentation on Property JSON Converters including:
- Complete interface documentation
- Multiple real-world examples
- Testing strategies
- Troubleshooting guide
- Advanced features

See: **[Property JSON Converters Guide](property-converters.md)**

## Tool Packs

Tool Packs are groups of related tools that can be loaded together. They help organize tools and provide instructions to the AI.

### Built-in Tool Packs

| Pack ID | Scope | Tools | Purpose |
|---------|-------|-------|---------|
| `WorkContentToolPack` | Chat | Read, search, get properties | Working with existing content |
| `UpdateToolPack` | Chat | Update properties, create drafts | Modifying content |
| `PublishToolPack` | Chat | Publish, unpublish, schedule | Publishing operations |
| `CreateToolPack` | Chat | Create pages, blocks | Creating new content |
| `ImageToolPack` | Chat | Analyze, generate alt text | Image operations |

### Creating a Custom Tool Pack

Implement `IToolPack`:

```csharp
using Epicweb.Optimizely.AIAssistant;

public class ProductToolPack : IToolPack
{
    public string Id => "ProductTools";
    public ToolScope Scope => ToolScope.Chat;
    
    public string Instructions => @"
# Product Tools

These tools help you work with product information:

- GetProductInfo - Get detailed product data
- CompareProducts - Compare multiple products
- UpdateProductPrice - Update product pricing

Use these when users ask about products, pricing, or comparisons.

## Examples
- ""What's the price of P12345?""
- ""Compare products P12345 and P67890""
- ""Update price for P12345 to $99.99""
";
}
```

Register in `Startup.cs`:

```csharp
services.AddSingleton<IToolPack, ProductToolPack>();
```

Mark your tools with the pack ID:

```csharp
[McpServerToolType]
public static class ProductTools
{
    [McpServerTool(Name = "GetProductInfo")]
    [ToolScope(ToolScope.Chat, PackageId = "ProductTools")]
    [Description("Get product information")]
    public static ProductInfo GetProductInfo(string productNumber)
    {
        // Implementation
    }
}
```

### Loading Tool Packs

#### From Chat

```
User: "Show available tool packs"
AI: "Available packs: WorkContent, Update, Publish, Create, Image, ProductTools"

User: "Load ProductTools"
AI: [Loads ProductTools pack] "Product tools loaded. You can now ask about products."
```

#### Automatic Loading

The AI automatically loads relevant tool packs based on the request:

```
User: "Update the heading on this page"
AI: [Automatically loads Update tool pack]
AI: "What should the new heading be?"
```



## Context vs Tools

Understanding when to use context vs tools is crucial for optimal performance and cost.

### Context (What to Send Always)

Context should be **small, stable, and declarative**:

```json
{
  "contentId": "123",
  "contentType": "ProductPage",
  "contentName": "Eco Water Bottle",
  "language": "en",
  "currentProperty": "MainBody",
  "selectedText": "...",
  "availableTools": ["GetContentProperty", "UpdateContent"]
}
```

### Tools (What to Fetch On-Demand)

Tools should be used for:
- Fetching actual content data
- Performing operations
- Accessing external systems

### The Lazy Loading Pattern

**❌ Don't send everything up front:**
```csharp
// Bad: High token cost, privacy risk
var context = new {
    fullPageHtml = page.GetFullHtml(),
    allProperties = page.Properties,
    childPages = GetAllChildren(page),
    mediaFiles = GetAllMedia(page)
};
```

**✅ Do use tools for on-demand data:**
```csharp
// Good: Small context, tools fetch what's needed
var context = new {
    contentId = page.ContentLink.ID,
    contentType = page.ContentTypeName,
    language = page.Language
};

// AI calls tools only when needed:
// - GetContentProperty("MainBody") - if needs content
// - GetChildContent() - if needs children
// - GetMediaFiles() - if needs media
```

### Benefits of Lazy Loading

1. **Lower Token Costs** - Only send what's needed
2. **Better Performance** - Less data transfer
3. **Improved Privacy** - Don't expose unnecessary data
4. **Better Reasoning** - AI focuses on relevant information
5. **More Scalable** - Works with large content structures

## Tool Execution Flow

### 1. User Request
```
User: "Create an SEO title for this page"
```

### 2. AI Analyzes Request
The AI determines it needs:
- Current page content
- The UpdateContent tool

### 3. AI Calls Tool
```json
{
  "tool": "GetThisContent",
  "parameters": {
    "contentId": "123",
    "properties": ["Heading", "MainBody", "Preamble"]
  }
}
```

### 4. Tool Executes
```csharp
public static ContentData GetThisContent(string contentId, string[] properties)
{
    var content = contentLoader.Get<IContent>(new ContentReference(contentId));
    var result = new ContentData();
    
    foreach (var prop in properties)
    {
        result.Properties[prop] = content.Property[prop]?.Value?.ToString();
    }
    
    return result;
}
```

### 5. AI Receives Result
```json
{
  "Heading": "Welcome",
  "MainBody": "This is the main content...",
  "Preamble": "A brief introduction..."
}
```

### 6. AI Generates Response
```
AI: "Based on your content, here's an SEO title:
'Welcome to Eco-Friendly Solutions | Sustainable Products'

Should I apply this?"
```

### 7. User Confirms
```
User: "Yes, apply it"
```

### 8. AI Calls Update Tool
```json
{
  "tool": "UpdateContentProperty",
  "parameters": {
    "contentId": "123",
    "propertyName": "MetaTitle",
    "value": "Welcome to Eco-Friendly Solutions | Sustainable Products"
  }
}
```

### 9. Confirmation
```
AI: "SEO title updated successfully!"
```

## Best Practices

### Tool Design

#### ✅ Do's
- **Single responsibility** - Each tool does one thing well
- **Clear descriptions** - AI needs to understand when to use it
- **Validate inputs** - Check parameters before execution
- **Return structured data** - Use classes, not strings
- **Handle errors gracefully** - Return meaningful error messages

#### ❌ Don'ts
- **Don't return HTML** - Use structured data
- **Don't auto-publish** - Always ask for confirmation
- **Don't assume permissions** - Check access rights
- **Don't make tools too generic** - Be specific
- **Don't ignore errors** - Log and report them

### Parameter Descriptions

Good parameter descriptions help the AI understand how to use tools:

```csharp
[McpServerTool(Name = "SearchProducts")]
[Description("Search for products by various criteria")]
public static List<ProductResult> SearchProducts(
    [Description("Search term to match against product name or description")]
    string searchTerm,
    
    [Description("Optional category filter (e.g., 'Electronics', 'Clothing')")]
    string category = null,
    
    [Description("Minimum price in USD")]
    decimal? minPrice = null,
    
    [Description("Maximum price in USD")]
    decimal? maxPrice = null,
    
    [Description("Maximum number of results to return (default: 10, max: 100)")]
    int limit = 10)
{
    // Implementation
}
```

### Error Handling

Return structured errors:

```csharp
[McpServerTool(Name = "UpdateProduct")]
public static ToolResult UpdateProduct(string productNumber, decimal newPrice)
{
    try
    {
        if (newPrice < 0)
            return ToolResult.Error("Price cannot be negative");
            
        var product = ProductRepository.GetByNumber(productNumber);
        if (product == null)
            return ToolResult.Error($"Product {productNumber} not found");
            
        product.Price = newPrice;
        ProductRepository.Save(product);
        
        return ToolResult.Success($"Updated {productNumber} to ${newPrice}");
    }
    catch (Exception ex)
    {
        return ToolResult.Error($"Failed to update: {ex.Message}");
    }
}

public class ToolResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    
    public static ToolResult Success(string message, object data = null) =>
        new() { Success = true, Message = message, Data = data };
        
    public static ToolResult Error(string message) =>
        new() { Success = false, Message = message };
}
```

### Security Considerations

1. **Validate Permissions**
```csharp
[McpServerTool(Name = "DeleteContent")]
public static ToolResult DeleteContent(string contentId, RequestModel context)
{
    var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
    var content = contentLoader.Get<IContent>(new ContentReference(contentId));
    
    // Check if user has delete permission
    if (!content.QueryDistinctAccess(AccessLevel.Delete))
        return ToolResult.Error("You don't have permission to delete this content");
        
    // Proceed with deletion
    // ...
}
```

2. **Sanitize Inputs**
```csharp
public static ToolResult UpdateContent(string contentId, string propertyName, string value)
{
    // Validate content ID format
    if (!ContentReference.TryParse(contentId, out var contentRef))
        return ToolResult.Error("Invalid content ID format");
        
    // Sanitize property name
    if (!IsValidPropertyName(propertyName))
        return ToolResult.Error("Invalid property name");
        
    // Sanitize value based on property type
    // ...
}
```

3. **Audit Operations**
```csharp
public static ToolResult PublishContent(string contentId)
{
    var logger = LogManager.GetLogger();
    logger.Information($"AI tool publishing content {contentId} by user {context.UserName}");
    
    // Publish operation
    // ...
}
```

## Testing Tools

### Unit Testing

```csharp
[Test]
public void GetProductInfo_ValidProduct_ReturnsInfo()
{
    // Arrange
    var productNumber = "P12345";
    
    // Act
    var result = ProductTools.GetProductInfo(productNumber);
    
    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(productNumber, result.ProductNumber);
    Assert.IsNotEmpty(result.Name);
}
```

### Integration Testing

Test tools through the AI Assistant:

1. Open chat
2. Type: `@GetProductInfo P12345`
3. Verify the AI receives correct data
4. Check logs for tool execution

### Debug Mode

Enable debug mode to see tool calls:

```json
{
  "Epicweb": {
    "AIAssistant": {
      "DebugMode": true
    }
  }
}
```

Chat will show tool execution details:
```
ℹ️ Calling tool: GetProductInfo
ℹ️ Parameters: { "productNumber": "P12345" }
ℹ️ Result: { "name": "Eco Bottle", "price": 29.99 }
```

## Real-World Examples

### Example 1: Product Catalog Integration

```csharp
[McpServerToolType]
public static class ProductCatalogTools
{
    [McpServerTool(Name = "GetProductDetails")]
    [ToolScope(ToolScope.Chat, PackageId = "ProductCatalog")]
    [Description("Get comprehensive product details including inventory")]
    public static async Task<ProductDetails> GetProductDetails(
        [Description("Product SKU or ID")] string productId)
    {
        var catalogService = ServiceLocator.Current
            .GetInstance<ICatalogService>();
            
        var product = await catalogService.GetProductAsync(productId);
        var inventory = await catalogService.GetInventoryAsync(productId);
        
        return new ProductDetails
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Currency = product.Currency,
            InStock = inventory.Quantity > 0,
            StockLevel = inventory.Quantity,
            Categories = product.Categories.ToList(),
            Images = product.Images.ToList(),
            Specifications = product.Specifications.ToDictionary()
        };
    }
    
    [McpServerTool(Name = "CompareProducts")]
    [ToolScope(ToolScope.Chat, PackageId = "ProductCatalog")]
    [Description("Compare features and specs of multiple products")]
    public static async Task<ProductComparison> CompareProducts(
        [Description("List of product IDs to compare")] 
        List<string> productIds)
    {
        var products = new List<ProductDetails>();
        
        foreach (var id in productIds)
        {
            products.Add(await GetProductDetails(id));
        }
        
        return new ProductComparison
        {
            Products = products,
            CommonFeatures = FindCommonFeatures(products),
            Differences = FindDifferences(products),
            PriceRange = new PriceRange
            {
                Min = products.Min(p => p.Price),
                Max = products.Max(p => p.Price)
            }
        };
    }
}
```

Usage:
```
User: "Compare products P123, P456, and P789"
AI: [Calls CompareProducts(["P123", "P456", "P789"])]
AI: "Here's a comparison table: ..."
```

### Example 2: External API Integration

```csharp
[McpServerToolType]
public static class WeatherTools
{
    [McpServerTool(Name = "GetWeatherForLocation")]
    [ToolScope(ToolScope.Chat, PackageId = "Weather")]
    [Description("Get current weather for a location")]
    public static async Task<WeatherData> GetWeatherForLocation(
        [Description("City name or coordinates")] string location)
    {
        using var client = new HttpClient();
        var apiKey = Configuration["WeatherAPI:Key"];
        var url = $"https://api.weather.com/current?location={location}&key={apiKey}";
        
        var response = await client.GetStringAsync(url);
        var data = JsonSerializer.Deserialize<WeatherApiResponse>(response);
        
        return new WeatherData
        {
            Location = location,
            Temperature = data.Temp,
            Condition = data.Conditions,
            Humidity = data.Humidity,
            WindSpeed = data.Wind
        };
    }
}
```

Usage:
```
User: "What's the weather in Stockholm?"
AI: [Calls GetWeatherForLocation("Stockholm")]
AI: "Current weather in Stockholm: 15°C, Partly cloudy, 65% humidity"
```

### Example 3: SEO Bulk Operations

```csharp
[McpServerToolType]
public static class SeoTools
{
    [McpServerTool(Name = "AnalyzeSeoForPages")]
    [ToolScope(ToolScope.Chat, PackageId = "SEO")]
    [Description("Analyze SEO quality for multiple pages")]
    public static async Task<SeoAnalysisResult> AnalyzeSeoForPages(
        [Description("List of page IDs to analyze")] List<string> pageIds)
    {
        var results = new List<PageSeoAnalysis>();
        var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        
        foreach (var pageId in pageIds)
        {
            var page = contentLoader.Get<PageData>(new ContentReference(pageId));
            var analysis = AnalyzePage(page);
            results.Add(analysis);
        }
        
        return new SeoAnalysisResult
        {
            TotalPages = results.Count,
            PassedPages = results.Count(r => r.Score >= 80),
            FailedPages = results.Count(r => r.Score < 50),
            AverageScore = results.Average(r => r.Score),
            Details = results
        };
    }
    
    private static PageSeoAnalysis AnalyzePage(PageData page)
    {
        var analysis = new PageSeoAnalysis
        {
            PageId = page.ContentLink.ID.ToString(),
            PageName = page.Name,
            Score = 0,
            Issues = new List<string>()
        };
        
        // Check meta title
        var metaTitle = page.Property["MetaTitle"]?.Value?.ToString();
        if (string.IsNullOrEmpty(metaTitle))
        {
            analysis.Issues.Add("Missing meta title");
        }
        else if (metaTitle.Length > 60)
        {
            analysis.Issues.Add("Meta title too long (>60 chars)");
        }
        else
        {
            analysis.Score += 25;
        }
        
        // Check meta description
        var metaDesc = page.Property["MetaDescription"]?.Value?.ToString();
        if (string.IsNullOrEmpty(metaDesc))
        {
            analysis.Issues.Add("Missing meta description");
        }
        else if (metaDesc.Length > 160)
        {
            analysis.Issues.Add("Meta description too long (>160 chars)");
        }
        else
        {
            analysis.Score += 25;
        }
        
        // Check heading structure
        var mainBody = page.Property["MainBody"]?.Value?.ToString();
        if (!string.IsNullOrEmpty(mainBody))
        {
            if (mainBody.Contains("<h1"))
                analysis.Score += 15;
            else
                analysis.Issues.Add("Missing H1 heading");
                
            if (mainBody.Contains("<h2"))
                analysis.Score += 10;
            else
                analysis.Issues.Add("Missing H2 headings");
        }
        
        // Check images alt text
        if (!string.IsNullOrEmpty(mainBody) && mainBody.Contains("<img"))
        {
            if (!mainBody.Contains("alt=\"\""))
                analysis.Score += 25;
            else
                analysis.Issues.Add("Images missing alt text");
        }
        
        return analysis;
    }
}
```

Usage:
```
User: "Analyze SEO for all product pages"
AI: [Gets product page IDs]
AI: [Calls AnalyzeSeoForPages([...ids])]
AI: "SEO Analysis: 23 pages, average score 67/100. 
    Common issues: 15 pages missing meta descriptions..."
```

## Demo Video

See AI Tools in action:

https://github.com/user-attachments/assets/726e2cb3-11fe-4ef7-8bf9-fafa22a9a1c1

## Troubleshooting

### Tool Not Found

**Problem:** AI says "I don't have access to that tool"

**Solutions:**
1. Check tool is registered in `Startup.cs`
2. Verify tool scope matches usage (Fields vs Chat)
3. Check tool name matches exactly
4. Ensure `[McpServerTool]` attribute is present

### Tool Execution Fails

**Problem:** Tool throws exceptions or returns errors

**Solutions:**
1. Check parameter types match expectations
2. Verify service dependencies are available
3. Add error handling and logging
4. Test tool in isolation with unit tests

### Tool Returns Wrong Data

**Problem:** Tool executes but returns incorrect results

**Solutions:**
1. Verify content ID format
2. Check language/culture context
3. Ensure permissions are correct
4. Add debug logging to trace execution

### Performance Issues

**Problem:** Tools are slow or timeout

**Solutions:**
1. Use async methods for I/O operations
2. Add caching for expensive operations
3. Limit result set sizes
4. Consider pagination for large data sets

## Next Steps

- **Explore Chat Features** - Learn about [AI Chat](chat-instructions.md)
- **Read Examples** - Check out real-world tool implementations
- **Watch Videos** - [Video tutorials on tools](https://aiassistant.optimizely.blog/en/videos/)
- **Join Community** - [GitHub Discussions](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)

## Additional Resources

- [Main Documentation](README.md)
- [Installation Guide](installation.md)
- [Chat Instructions Guide](chat-instructions.md)
- [Configuration Guide](configuration.md)
- [Tool Architecture (Technical)](tools-vs-context.md)
- [Official Blog](https://aiassistant.optimizely.blog)

---

**Need Help?**
- [GitHub Discussions](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/discussions)
- [Report Issues](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/issues)
- [Video Guides](https://aiassistant.optimizely.blog/en/videos/)
