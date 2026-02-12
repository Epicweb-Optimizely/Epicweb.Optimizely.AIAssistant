# Property JSON Converters

Property JSON Converters (`IPropertyJsonConverter`) are a powerful feature that allows you to teach the AI how to properly format JSON for complex Optimizely property types. This is essential when working with content updates through AI tools, enabling the AI to understand and correctly handle properties like `ContentArea`, `ContentReference`, `XhtmlString`, and custom business types.

## Table of Contents

- [Overview](#overview)
- [Why Use Property Converters?](#why-use-property-converters)
- [The IPropertyJsonConverter Interface](#the-ipropertyjsonconverter-interface)
- [When to Create a Converter](#when-to-create-a-converter)
- [Built-in Converters](#built-in-converters)
- [Creating Custom Converters](#creating-custom-converters)
- [Registering Converters](#registering-converters)
- [How Converters Work](#how-converters-work)
- [Best Practices](#best-practices)
- [Instructions Template](#instructions-template)
- [Testing Converters](#testing-converters)
- [Troubleshooting](#troubleshooting)
- [Advanced Features](#advanced-features)
- [Real-World Examples](#real-world-examples)

## Overview

Property JSON Converters bridge the gap between the AI's understanding and Optimizely's complex property types. They provide:

- **AI Instructions** - Clear guidelines on how to format JSON
- **JSON Schema** - Validation constraints for the AI
- **Bidirectional Conversion** - Transform between JSON and CLR types
- **Type Safety** - Ensure data integrity during conversion
- **Property Descriptions** - Used by AI tools to understand content structure

### How Converters Are Used

Property JSON Converters are automatically invoked in several scenarios:

#### 1. Content Property Descriptions

When AI tools like `GetContentProperties` retrieve property metadata, converters provide formatting instructions:

```csharp
// GetContentProperties tool execution
var properties = GetContentProperties(contentId);

// For each property, if a converter exists:
// {
//   "propertyName": "Categories",
//   "propertyType": "IList<string>",
//   "currentValue": ["Electronics", "Computers"],
//   "formatInstructions": "[Converter's GetInstructions() output]",
//   "jsonSchema": "[Converter's GetJsonSchema() output]"
// }
```

This allows the AI to understand:
- What the property contains
- How to format updates
- What values are valid
- Common mistakes to avoid

#### 2. Property Update Operations

When AI tools update content properties, converters handle the conversion:

```csharp
// UpdateContentProperty tool execution
public static ToolResult UpdateContentProperty(
    string contentId, 
    string propertyName, 
    JsonElement value)
{
    // 1. Get property info
    var propertyInfo = GetPropertyInfo(contentId, propertyName);
    
    // 2. Find matching converter
    var converter = FindConverter(propertyInfo);
    
    // 3. Use converter to transform JSON to CLR object
    var clrValue = converter?.Convert(value, propertyInfo);
    
    // 4. Assign to property
    content.Property[propertyName].Value = clrValue;
}
```

#### 3. Property Reading Operations

When AI tools read current property values, converters serialize to AI-friendly format:

```csharp
// Reading property value
var currentValue = content.Property["Categories"].Value;

// Find converter and serialize
var converter = FindConverter(propertyInfo);
var jsonValue = converter?.ConvertValueToJson(currentValue, options);

// Return to AI in understandable format
return new {
    propertyName = "Categories",
    value = jsonValue  // ["Electronics", "Computers"]
};
```

### Integration with AI Tools

The `GetContentProperties` tool is a primary consumer of property converters:

```csharp
[McpServerTool(Name = "GetContentProperties")]
[Description("Get detailed information about content properties including format instructions")]
public static PropertyDescriptions GetContentProperties(
    [Description("Content ID")] string contentId,
    [Description("Optional: specific properties to describe")] string[] propertyNames = null)
{
    var content = contentLoader.Get<IContent>(new ContentReference(contentId));
    var descriptions = new List<PropertyDescription>();

    foreach (var property in content.Properties)
    {
        var propertyInfo = GetPropertyInfo(content, property.Name);
        var converter = FindConverter(propertyInfo);

        descriptions.Add(new PropertyDescription
        {
            Name = property.Name,
            Type = property.PropertyValueType.Name,
            CurrentValue = converter?.ConvertValueToJson(property.Value, options),
            FormatInstructions = converter?.GetInstructions(propertyInfo),
            JsonSchema = converter?.GetJsonSchema(propertyInfo),
            IsRequired = IsRequired(propertyInfo),
            IsReadOnly = property.IsReadOnly
        });
    }

    return new PropertyDescriptions { Properties = descriptions };
}
```

**Example AI Tool Usage:**

```
User: "What properties does this page have?"
AI: [Calls GetContentProperties("123")]
AI: "This page has the following properties:

- **Categories** (IList<string>)
  - Current value: ["Electronics", "Computers"]
  - Format: Array of category name strings
  - Rules: 1-5 items, no duplicates

- **MainContentArea** (ContentArea)
  - Current value: [Block 45, Block 67]
  - Format: Array of content references
  - Rules: Each item needs contentLink or contentGuid

Would you like to update any of these properties?"
