using System;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization.Json;

namespace RunKeeper.Client
{
    /// <summary>
    /// Responsible for the communication with the 
    /// runkeeper endpoints.
    /// </summary>
    internal class RunKeeperWebProxy
    {
        public Stream Post(string url, string contentType, string body)
        {
            var requestObject = GetPostRequest(url, contentType, body);

            return GetResponse(requestObject);
        }

        protected virtual Stream GetResponse(HttpWebRequest requestObject)
        {
            return requestObject.GetResponse().GetResponseStream();
        }

        protected virtual HttpWebRequest GetPostRequest(string url, string contentType, string body)
        {
            var requestObject = GetPostRequestObject(url, contentType);

            WriteBodyToRequestObject(body, requestObject);

            return requestObject;
        }

        private static HttpWebRequest GetPostRequestObject(string url, string contentType)
        {
            var requestObject = (HttpWebRequest)HttpWebRequest.Create(url);
            requestObject.Method = "POST";
            requestObject.ContentType = contentType;

            return requestObject;
        }

        private void WriteBodyToRequestObject(string body, HttpWebRequest requestObject)
        {
            using (var streamWriter = new StreamWriter(GetRequestStream(requestObject)))
            {
                streamWriter.Write(body);                
            }
        }

        protected virtual Stream GetRequestStream(HttpWebRequest requestObject)
        {
            return requestObject.GetRequestStream();
        }

        /// <summary>
        /// Should be used only if you really need to reponse stream directly. In all other sitatuions the
        /// generic version with buildt in de-serialization should be used.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public virtual Stream Get(string endpoint, NameValueCollection headers)
        {
            Contract.Requires(!String.IsNullOrEmpty(endpoint));
            Contract.Requires(endpoint.StartsWith("/", StringComparison.OrdinalIgnoreCase));
            Contract.Requires(headers != null);

            var request = (HttpWebRequest)HttpWebRequest.Create("https://api.runkeeper.com" + endpoint);

            SetRequestHeaders(headers, request);

            return GetResponse(request);
        }

        public virtual T Get<T>(string endpoint, NameValueCollection headers)
        {           
            using (var responseStream = Get(endpoint, headers))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(responseStream);
            }
        }


        private static void SetRequestHeaders(NameValueCollection headers, HttpWebRequest request)
        {
            var acceptHeader = headers["Accept"];
            if (!String.IsNullOrEmpty(acceptHeader))
            {
                headers.Remove("Accept");
                request.Accept = acceptHeader;
            }

            request.Headers.Add(headers);
            request.Method = "GET";
        }
    }
}
