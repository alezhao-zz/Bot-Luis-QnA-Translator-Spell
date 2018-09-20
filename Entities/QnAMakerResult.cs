using System;
using System.Collections.Generic;

namespace Microsoft.Bot.Sample.SimpleEchoBot.Entities
{
    [Serializable]
    public class QnAMakerResult
    {
        public List<Answer> answers { get; set; }
    }

    [Serializable]
    public class Answer
    {
        public string answer { get; set; }
        public List<string> questions { get; set; }
        public double score { get; set; }
    }

}