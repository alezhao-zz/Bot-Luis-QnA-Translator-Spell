namespace Microsoft.Bot.Sample.SimpleEchoBot.Helper
{
    public class BingSpellCheckFlaggedToken
    {
        public int Offset { get; set; }

        public string Token { get; set; }

        public string Type { get; set; }

        public BingSpellCheckSuggestion[] Suggestions { get; set; }
    }
}
