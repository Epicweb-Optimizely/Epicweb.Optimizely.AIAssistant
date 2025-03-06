# Image generation with Recraft AI

This integration brings the next generation of image generation capabilities directly into your content management system, revolutionizing the way you create and manage visual assets.

Blog posts: https://optimizely.blog/?tag=RecraftAI

## Get started

You will need an account on recraft.ai => https://www.recraft.ai/profile/api and then purchase API credits. 


## appsettings config: 

```
      //"ImageProviderName": "RecraftAIOpenAI",//use this one in combination with OpenAI.com
      "ImageProviderName": "RecraftAI", //use this one in combination with Azure OpenAI Services
      "ApiKeyImage": "a65BEDVYTO..............", //retrieved from https://www.recraft.ai/profile/api
      "ImageModel": "recraftv3", //or recraft20b
```

# Config Size, Actions, Styles and substyle

Find all styles here: https://www.recraft.ai/docs#list-of-styles

```
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
      "ImageActions":       "ImageActions": [
        {
          "id": "removeBackground", //case sensitive
          "name": "Remove Background"
        },
        {
          "id": "replaceBackground", //case sensitive
          "name": "Replace Background",
          "prompt": true
        },
        {
          "id": "imageToImage", //case sensitive
          "name": "Generate variation",
          "prompt": true
        }
      ]
```


## AIAssistant Attributes on properties

This is a hack, but you can add a querystring in the ImageGenerationStyle for default behavior, the user can override this in UI

It will add top level parameters to the request https://www.recraft.ai/docs#generate-image

```
    [UIHint(AIHint.Image)]
    [AIAssistant(ImageGenerationStyle = "style=digital_illustration|cosmics&model=recraftv3", ImageGenerationSize = "1820x1024")]
    public virtual ContentReference Image90 { get; set; }
```
