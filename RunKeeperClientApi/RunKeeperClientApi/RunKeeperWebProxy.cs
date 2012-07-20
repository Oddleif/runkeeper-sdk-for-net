using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            HttpWebRequest requestObject = (HttpWebRequest)HttpWebRequest.Create(url);
            requestObject.Method = "POST";
            requestObject.ContentType = contentType;

            using (var streamWriter = new StreamWriter(requestObject.GetRequestStream()))
            {
                streamWriter.Write(body);
            }

            return requestObject;
        }

        public virtual string Get(Uri url, NameValueCollection headers)
        {
            Contract.Requires(url != null);
            Contract.Requires(headers != null);

            var request = (HttpWebRequest)HttpWebRequest.Create(url);

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
