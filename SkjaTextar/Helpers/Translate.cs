using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Threading;
using SkjaTextar.Models;

namespace SkjaTextar.Helpers
{
    // Code from:
    // http://msdn.microsoft.com/en-us/library/ff512435.aspx
    public class Translate
    {
        public static void TranslateText(Translation trans, string to, string from = "en")
        {
            AdmAccessToken admToken;
            string headerValue;
            //Get Client Id and Client Secret from https://datamarket.azure.com/developer/applications/
            //Refer obtaining AccessToken (http://msdn.microsoft.com/en-us/library/hh454950.aspx) 
            AdmAuthentication admAuth = new AdmAuthentication("transtest01", "1Z2aNoTqwiJiTDmjCNwA5JOqyP2hCeff54oYaeKSmNw=");
            try
            {
                admToken = admAuth.GetAccessToken();
                // Create a header with the access_token property of the returned token
                headerValue = "Bearer " + admToken.access_token;
                //DetectMethod(headerValue);
                foreach (var segment in trans.TranslationSegments)
                {
                    if (!String.IsNullOrEmpty(segment.Original1))
                    {
                        segment.Line1 = TranslateMethod(headerValue, segment.Original1, to);
                    }
                    if (!String.IsNullOrEmpty(segment.Original2))
                    {
                        segment.Line2 = TranslateMethod(headerValue, segment.Original2, to);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private static String TranslateMethod(string authToken, string toTranslate, string to, string from = "en")
        {
            // Add TranslatorService as a service reference, Address:http://api.microsofttranslator.com/V2/Soap.svc
            TranslatorService.LanguageServiceClient client = new TranslatorService.LanguageServiceClient();
            //Set Authorization header before sending the request
            HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Method = "POST";
            httpRequestProperty.Headers.Add("Authorization", authToken);
            string translationResult = null;
            // Creates a block within which an OperationContext object is in scope.
            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                //Keep appId parameter blank as we are sending access token in authorization header.
                translationResult = client.Translate("", toTranslate, from, to, "text/plain", "general");
            }
            return translationResult;
        }
    }
    [DataContract]
    public class AdmAccessToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }

    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string clientId;
        private string clientSecret;
        private string request;
        private AdmAccessToken token;
        private Timer accessTokenRenewer;

        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;

        public AdmAuthentication(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            this.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
            this.token = HttpPost(DatamarketAccessUri, this.request);
            //renew the token every specfied minutes
            accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }

        public AdmAccessToken GetAccessToken()
        {
            return this.token;
        }


        private void RenewAccessToken()
        {
            AdmAccessToken newAccessToken = HttpPost(DatamarketAccessUri, this.request);
            //swap the new token with old one
            //Note: the swap is thread unsafe
            this.token = newAccessToken;
            Console.WriteLine(string.Format("Renewed token for user: {0} is: {1}", this.clientId, this.token.access_token));
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception)
                {
                    
                }
            }
        }


        private AdmAccessToken HttpPost(string DatamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(DatamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
            webRequest.ContentLength = bytes.Length;
            using (Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }
}