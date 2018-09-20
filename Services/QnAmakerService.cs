using Microsoft.Bot.Sample.SimpleEchoBot.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace Microsoft.Bot.Sample.SimpleEchoBot.Services
{
    [Serializable]
    public class QnAmakerService
    {
        public QnAMakerResult GetMessageFromQnAMaker(string message)
        {
            QnAMakerResult response;
            string responseString = string.Empty;

            var knowledgebaseId = WebConfigurationManager.AppSettings["KnowledgeBaseId"]; 
            var qnamakerAuthorizationKey = WebConfigurationManager.AppSettings["AuthorizationKey"];
            var qnamakerUribase = WebConfigurationManager.AppSettings["QnAmakerUriBase"]; 

            //Build the URI
            Uri qnamakerUri = new Uri(qnamakerUribase);
            var builder = new UriBuilder($"{qnamakerUri}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{message}\"}}";

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Authorization", qnamakerAuthorizationKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);
            }

            try
            {
                response = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
            }
            catch
            {
                throw new Exception("Unable to deserialize QnA Maker response string.");
            }

            return response;
        }
    }
}