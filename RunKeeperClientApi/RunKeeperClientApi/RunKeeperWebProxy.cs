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
        public virtual Stream Post(string url, string contentType, string body)
        {
            var requestObject = GetPostRequest(url, contentType, body);

            return GetResponse(requestObject);
        }

        private static Stream GetResponse(HttpWebRequest requestObject)
        {
            var response = requestObject.GetResponse();

            return response.GetResponseStream();
        }

        private static HttpWebRequest GetPostRequest(string url, string contentType, string body)
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
       
            var response = request.GetResponse();
            
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
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
