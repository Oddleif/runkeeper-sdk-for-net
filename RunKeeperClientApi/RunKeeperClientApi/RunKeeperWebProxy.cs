using System;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics.Contracts;

namespace RunKeeperClientApi
{
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

        private static void WriteBodyToRequestObject(string body, HttpWebRequest requestObject)
        {
            using (var streamWriter = new StreamWriter(requestObject.GetRequestStream()))
            {
                streamWriter.Write(body);
            }
        }

        public virtual string Get(string endpoint, NameValueCollection headers)
        {
            Contract.Requires(!String.IsNullOrEmpty(endpoint));
            Contract.Requires(endpoint.StartsWith("/"));
            Contract.Requires(headers != null);

            var request = (HttpWebRequest)HttpWebRequest.Create("https://api.runkeeper.com" + endpoint);

            SetRequestHeaders(headers, request);

            return GetResponseBody(request);
        }

        private string GetResponseBody(HttpWebRequest request)
        {
            using (var streamReader = new StreamReader(GetResponseStream(request)))
            {
                return streamReader.ReadToEnd();
            }
        }

        protected virtual Stream GetResponseStream(HttpWebRequest request)
        {
            return request.GetResponse().GetResponseStream();
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
