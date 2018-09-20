namespace Microsoft.Bot.Sample.SimpleEchoBot.Entity
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class RestaurantsQuery
    {
        //[Prompt("Please enter your {&}")]
        [Optional]
        public string PlaceName { get; set; }

        //[Prompt("Near which Address")]
        [Optional]
        public string Address { get; set; }
    }
}