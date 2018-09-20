using Microsoft.Bot.Sample.SimpleEchoBot.App_Start;
using Microsoft.Bot.Sample.SimpleEchoBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Microsoft.Bot.Sample.SimpleEchoBot.Services
{
    [Serializable]
    public class TranslatorService
    {
        private static readonly string strAzureTranslatorApiKey = WebConfigurationManager.AppSettings["AzureTranslatorApiKey"];
        public async Task<string> TranslatorAsync(string to, string content)
        {
            string returnContent = string.Empty;
            string fromType = await TranslatorDetect(content);
            if (fromType == to) return content;
            returnContent = await TranslatorExecute(fromType, to, content);
            return returnContent;
        }

        public async Task<string> TranslatorExecute(string from, string to, string content)
        {
            string returnContent = string.Empty;
            var authTokenSource = new AzureAuthToken(strAzureTranslatorApiKey);
            string authToken;
            authToken = await authTokenSource.GetAccessTokenAsync();
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(content) + "&from=" + from + "&to=" + to;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            using (WebResponse res = httpWebRequest.GetResponse())
            using (Stream stream = res.GetResponseStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                returnContent = (string)dcs.ReadObject(stream);
            }
            return returnContent;
        }

        public async Task<string> TranslatorDetect(string content)
        {
            string returnContentType = string.Empty;
            var authTokenSource = new AzureAuthToken(strAzureTranslatorApiKey);
            string authToken;
            authToken = await authTokenSource.GetAccessTokenAsync();
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Detect?text=" + content;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            using (WebResponse response = httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                returnContentType = (string)dcs.ReadObject(stream);
            }
            return returnContentType;
        }
    }
}