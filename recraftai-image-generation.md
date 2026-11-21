# Image generation with Recraft AI

This page records the Recraft integration settings used by the earlier image editor setup. The plugin still passes these options to the AI service. Confirm the provider and model names supported by your AI service before reusing the example values.

Blog posts: https://optimizely.blog/?tag=RecraftAI

## Get started

Use your Recraft API credentials with the image provider configured for your AI service. [Recraft API account](https://www.recraft.ai/profile/api)


## Application settings

Configure image options under `Epicweb:AIAssistant`. The earlier integration used provider names such as `RecraftAI` and `RecraftAIOpenAI`. Use the provider and model names accepted by your AI service.

```json
{
  "Epicweb": {
    "AIAssistant": {
      "ImageProviderName": "RecraftAI",
      "ApiKeyImage": "<recraft-api-key>",
      "ImageModel": "recraftv3"
    }
  }
}
```

## Image selections and actions

The plugin passes `ImageSelectionValues` and `ImageActions` through when the image dialog initializes. Merge the following example settings into `Epicweb:AIAssistant`. Action IDs are case-sensitive. The style and size values come from the earlier Recraft setup. [Recraft style reference](https://www.recraft.ai/docs#list-of-styles)

```json
{
  "ImageSelectionValues": [
        {
          "property": "style",
          "name": "realistic_image",
          "model": "recraftv3",
          "values": [
            "",
            "b_and_w",
            "enterprise",
            "evening_light",
            "faded_nostalgia",
            "forest_life",
            "hard_flash",
            "hdr",
            "motion_blur",
            "mystic_naturalism",
            "natural_light",
            "natural_tones",
            "organic_calm",
            "real_life_glow",
            "retro_realism",
            "retro_snapshot",
            "studio_portrait",
            "urban_drama",
            "village_realism",
            "warm_folk"
          ]
        },
        {
          "property": "style",
          "name": "digital_illustration",
          "model": "recraftv3",
          "values": [
            "",
            "2d_art_poster",
            "2d_art_poster_2",
            "engraving_color",
            "grain",
            "hand_drawn",
            "hand_drawn_outline",
            "handmade_3d",
            "infantile_sketch",
            "pixel_art",
            "antiquarian",
            "bold_fantasy",
            "child_book",
            "child_books",
            "cover",
            "crosshatch",
            "digital_engraving",
            "expressionism",
            "freehand_details",
            "grain_20",
            "graphic_intensity",
            "hard_comics",
            "long_shadow",
            "modern_folk",
            "multicolor",
            "neon_calm",
            "noir",
            "nostalgic_pastel",
            "outline_details",
            "pastel_gradient",
            "pastel_sketch",
            "pop_art",
            "pop_renaissance",
            "street_art",
            "tablet_sketch",
            "urban_glow",
            "urban_sketching",
            "vanilla_dreams",
            "young_adult_book",
            "young_adult_book_2"
          ]
        },
        {
          "property": "style",
          "name": "vector_illustration",
          "model": "recraftv3",
          "values": [
            "",
            "bold_stroke",
            "chemistry",
            "colored_stencil",
            "contour_pop_art",
            "cosmics",
            "cutout",
            "depressive",
            "editorial",
            "emotional_flat",
            "engraving",
            "infographical",
            "line_art",
            "line_circuit",
            "linocut",
            "marker_outline",
            "mosaic",
            "naivector",
            "roundish_flat",
            "segmented_colors",
            "sharp_contrast",
            "thin",
            "vector_photo",
            "vivid_shapes"
          ]
        },
        {
          "property": "style",
          "name": "realistic_image",
          "model": "recraft20b",
          "values": [
            "",
            "b_and_w",
            "enterprise",
            "hard_flash",
            "hdr",
            "motion_blur",
            "natural_light",
            "studio_portrait"
          ]
        },
        {
          "property": "style",
          "name": "digital_illustration",
          "model": "recraft20b",
          "values": [
            "",
            "2d_art_poster",
            "2d_art_poster_2",
            "3d",
            "80s",
            "engraving_color",
            "glow",
            "grain",
            "hand_drawn",
            "hand_drawn_outline",
            "handmade_3d",
            "infantile_sketch",
            "kawaii",
            "pixel_art",
            "psychedelic",
            "seamless",
            "voxel",
            "watercolor"
          ]
        },
        {
          "property": "style",
          "name": "icon",
          "model": "recraft20b",
          "values": [
            "",
            "broken_line",
            "colored_outline",
            "colored_shapes",
            "colored_shapes_gradient",
            "doodle_fill",
            "doodle_offset_fill",
            "offset_fill",
            "outline",
            "outline_gradient",
            "uneven_fill"
          ]
        },
        {
          "property": "size",
          "name": "size",
          "values": [
            "1024x1024",
            "1365x1024",
            "1024x1365",
            "1536x1024",
            "1024x1536",
            "1820x1024",
            "1024x1820",
            "1024x2048",
            "2048x1024",
            "1434x1024",
            "1024x1434",
            "1024x1280",
            "1280x1024",
            "1024x1707",
            "1707x1024"
          ]
        }
      ],
      "ImageActions": [
        {
          "id": "removeBackground",
          "name": "Remove Background"
        },
        {
          "id": "replaceBackground",
          "name": "Replace Background",
          "prompt": true
        },
        {
          "id": "imageToImage",
          "name": "Generate variation",
          "prompt": true
        }
      ]
}
```

## AI Assistant attributes on properties

Use `ImageGenerationStyle` and `ImageGenerationSize` to set defaults for the property. Selections in the dialog take precedence over the property's style. The AI service interprets the style string for the provider.

```csharp
[UIHint(AIHint.Image)]
[AIAssistant(ImageGenerationStyle = "style=realistic_image", ImageGenerationSize = "1820x1024")]
public virtual ContentReference Image90 { get; set; }
```
