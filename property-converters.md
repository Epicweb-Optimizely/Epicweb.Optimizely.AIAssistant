# Property JSON Converters

The AI Assistant uses `IPropertyJsonConverter` to describe complex Optimizely properties to AI tools and convert between JSON and CLR values.

## How converters are used

`GetContentProperties` returns property metadata. For a property with a registered converter, the result includes the converter's `instructions` and `jsonSchema`. If property values are requested and the property is available to the AI, the tool returns the serialized value in `value` as JSON text. Set `includeContent` to `false` to request metadata without values.

The content creation tools and `UpdateMultiplePropertiesOnPageOrBlock` call the matching converter when assigning JSON to a supported property. The schema describes the expected payload. The converter's `Convert` implementation determines how the input is parsed and which values it accepts.

## Built-in converters

| Converter | Property type | Expected JSON |
| --- | --- | --- |
| `StringListConverter` | `IList<string>` | Array of strings such as `["red", "blue"]` |
| `LocalizedStringConverter` | Localized string collections | Array of objects such as `[{"language":"en","value":"Hello"}]` |
| `DefaultContentAreaConverter` | `ContentArea` | Array of content IDs or item objects with keys such as `contentLink`, `displayOption`, or `inlineBlock` |
| `DefaultCategoryListConverter` | `CategoryList` | Array of category IDs, names, or GUIDs |
| `TempBlockConverter` | `BlockData` properties | Exposes block type information when reading. Assigning a JSON value through this converter is unsupported. |

## Create and register a converter

Implement `IPropertyJsonConverter` in the `Epicweb.Optimizely.AIAssistant.Converters` namespace. Use `CanHandle(PropertyInfo)` to select the properties the converter supports. Implement `GetInstructions` and `GetJsonSchema` to describe the format, `Convert` to produce the assignable CLR value, and `ConvertValueToJson` to expose it to AI tools.

Register the implementation in dependency injection.

```csharp
using Epicweb.Optimizely.AIAssistant.Converters;

services.AddSingleton<IPropertyJsonConverter, MyPropertyConverter>();
```

The registry selects the first registered converter whose `CanHandle` returns `true`. To override a built-in converter for a matching property, register the custom converter before calling `AddAIAssistant()`.

See [AI tools](tools.md#property-json-converters) for how to use converters when developing tools.
