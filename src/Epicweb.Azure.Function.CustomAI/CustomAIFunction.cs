using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Epicweb.Azure.Function.CustomAI
{
    public class CustomAI
    {
        private readonly ILogger<CustomAI> _logger;

        public CustomAI(ILogger<CustomAI> logger)
        {
            _logger = logger;
        }

        [Function("CustomAI")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous /*AuthorizationLevel.Function*/, "post")] HttpRequest req)
        {
            string bearerToken = req.Headers["Authorization"].ToString();
            //todo implement your logic, you may also use AuthorizationLevel.Function above
            if (string.IsNullOrEmpty(bearerToken?.Replace("Bearer ","")))
            {
                _logger.LogError("Bearer Token is missing");
                return new UnauthorizedResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                _logger.LogError("Request Body is Empty");
                return new BadRequestResult();
            }

            AIRequest data = null;
            string errormessage = null;
            try
            {
                data = JsonSerializer.Deserialize<AIRequest>(requestBody);

            }
            catch (Exception e)
            {
                errormessage = e.Message;
            }
            
            //Here are the key data fields that you can access and manipulate within your custom AI function:

            //data.Prompt //The processed prompt.
            //data.Instructions //Instructions provided to the AI.
            //data.SystemPrompt //Additional instructions, rules, or terms for the AI.
            //data.Action //The requested action, which can be "default", "vision", or "imagegeneration".
            //data.Base64Image //A supplied image in Base64 format.

            //data.OriginalRequest //The original request from the plugin before processing by Epicweb.
            //data.OriginalRequest.Text //The original text/prompt from the plugin.
            //data.OriginalRequest.Prompt //Processed prompt from the plugin.
            //data.OriginalRequest.Shortcut //Name of the shortcut, if any
            //data.OriginalRequest.CustomJson //A string that needs to be serialized.
            //data.OriginalRequest.IsAuto //Indicates if the request is from an automatic process in Optimizely (e.g., LanguageManager).
            //data.OriginalRequest.IsHtml //if true use HTML in response, probably used in Rich Text Editor
            //data.OriginalRequest.CorrelationId //Used for support or troubleshooting along with the timestamp.
            //data.OriginalRequest.Lang // The language context of the request.

            //Implement your custom AI
            if (data != null)
            {
                if (data.Action == "default")
                {
                    var result = new AIResult()
                    {
                        //Text = $"CustomAI response: {data.OriginalRequest.CustomJson} {data.OriginalRequest.Text}",
                        Text = $"CustomAI response: {data.OriginalRequest.Text}",
                        Limitations = "Return a message to the user",
                        TotalTokens = 100
                    };

                    return new OkObjectResult(result);
                }
                else if (data.Action == "vision")
                {
                    //analize the image with data.Prompt and return the text
                    var result = new AIResult()
                    {
                        Text = $"a forest with the sun peeking thru the trees",
                        //Limitations = "Return a message to the user",
                        TotalTokens = 100
                    };

                    return new OkObjectResult(result);
                }
                else if (data.Action == "imagegeneration")
                {
                    //Generate your image with data.Prompt and then return the array of ImageResult
                    return new OkObjectResult(new ImageResult[] { new ImageResult() { Url = "https://gosso.se/Luc-Gosso-2023.jpg", RevisedPrompt = "Image of Luc Gosso" } });
                }
            }


            return new ObjectResult("Something went wrong in CustomAI function")
            {
                StatusCode = 500
            };
        }
    }
}
