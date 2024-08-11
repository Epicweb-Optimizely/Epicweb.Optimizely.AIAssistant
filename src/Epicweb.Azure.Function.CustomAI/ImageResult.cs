using System.Text.Json.Serialization;
#nullable disable

public class ImageResult
{
    [JsonInclude]
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonInclude]
    [JsonPropertyName("b64_json")]
    public string B64_Json { get; set; }

    [JsonInclude]
    [JsonPropertyName("revised_prompt")]
    public string RevisedPrompt { get; set; }   
}

