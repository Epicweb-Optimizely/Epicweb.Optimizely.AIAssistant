namespace Epicweb.Azure.Function.CustomAI
{

    public class AIResult : IAIResult
    {
        public string Message { get; set; }
        public long TotalTokens { get; set; }
        public string Text { get; set; }
        public string Limitations { get; set; }
    }
    public interface IAIResult
    {
        string Text { get; set; }
        long TotalTokens { get; set; }
        string Message { get; set; }
        /// <summary>
        /// If there is some limitation somehow thats needs to be supplied to the end user
        /// </summary>
        string Limitations { get; set; }
    }
}
