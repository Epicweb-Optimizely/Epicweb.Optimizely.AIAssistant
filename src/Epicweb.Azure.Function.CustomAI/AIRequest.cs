using System.Text.Json.Serialization;

public class AIRequest
{
    /// <summary>
    /// The instruction to the AI
    /// </summary>
    [JsonPropertyName("instructions")]
    public string Instructions { get; set; }

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    /// <summary>
    /// some additional instructions to the AI, rules or terms
    /// </summary>
    [JsonPropertyName("systemPrompt")]
    public string SystemPrompt { get; set; }

    [JsonPropertyName("maxTokens")]
    public int MaxTokens { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }

    /// <summary>
    /// Used for support or error findings together with time
    /// </summary>
    [JsonPropertyName("correlationId")]
    public string CorrelationId { get; set; }

    /// <summary>
    /// The original request from the plugin that has not been processed by Epicweb
    /// </summary>
    [JsonPropertyName("originalRequest")]
    public OriginalRequest OriginalRequest { get; set; }


    /// <summary>
    /// request action "default", "vision" or "imagegeneration"
    /// With vision a base64Image is sent with the request
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; }

    /// <summary>
    /// Image to process
    /// </summary>
    [JsonPropertyName("base64Image")]
    public string Base64Image { get; set; }
}

public class OriginalRequest
{
    /// <summary>
    /// the Original text/prompt from the plugin
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    /// <summary>
    /// Processed prompt from the plugin
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    /// <summary>
    /// Name of the shortcut, if used
    /// </summary>
    [JsonPropertyName("shortcut")]
    public string Shortcut { get; set; }
    /// <summary>
    /// If it is coming from an automatic process in Optimizely, for example LanguageManager
    /// </summary>
    [JsonPropertyName("isAuto")]
    public bool IsAuto { get; set; }

    /// <summary>
    /// Used for support or error findings together with time
    /// </summary>
    [JsonPropertyName("correlationId")]
    public string CorrelationId { get; set; }

    /// <summary>
    /// The version installed
    /// </summary>
    [JsonPropertyName("pluginVersion")]
    public string PluginVersion { get; set; }

    /// <summary>
    /// What domain used for the Optimizely Instance
    /// </summary>
    [JsonPropertyName("referringUrl")]
    public string ReferringUrl { get; set; }

    /// <summary>
    /// The language context used
    /// </summary>
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    /// <summary>
    /// if true use HTML in response, probably used in Rich Text Editor
    /// </summary>
    [JsonPropertyName("isHtml")]
    public bool IsHtml { get; set; }

    /// <summary>
    /// This is the account name, not the users username
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("assistantInstructions")]
    public string AssistantInstructions { get; set; }

    [JsonPropertyName("systemPrompt")]
    public string SystemPrompt { get; set; }

    [JsonPropertyName("maxTokens")]
    public int MaxTokens { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("visionmodel")]
    public string VisionModel { get; set; }

    [JsonPropertyName("clientkey")]
    public string ClientKey { get; set; }

    [JsonPropertyName("returnPrompt")]
    public bool ReturnPrompt { get; set; }

    [JsonPropertyName("serviceUrl")]
    public string ServiceUrl { get; set; }

    [JsonPropertyName("provider")]
    public string Provider { get; set; }

    /// <summary>
    /// A string, you need to serialize this
    /// </summary>
    [JsonPropertyName("customJson")]
    public object CustomJson { get; set; }

}
