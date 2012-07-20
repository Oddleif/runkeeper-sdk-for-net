using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics.Contracts;
using System.Net;

namespace RunKeeperClientApi
{
    internal class RunKeeperTestProxy : RunKeeperWebProxy
    {   
        protected override Stream GetResponse(System.Net.HttpWebRequest requestObject)
        {
            return new MemoryStream(Encoding.Default.GetBytes("{\"token_type\":\"Bearer\",\"access_token\":\"2ec59fa926d044bea8dc256174619625\"}"));
        }

        protected override System.Net.HttpWebRequest GetPostRequest(string url, string contentType, string body)
        {
            Contract.Ensures(Contract.Result<HttpWebRequest>().ContentType == contentType);
            Contract.Ensures(Contract.Result<HttpWebRequest>().Method == "POST");
            // TODO: Ensure body is written to request body

            return base.GetPostRequest(url, contentType, body);
        }

        protected override Stream GetResponseStream(HttpWebRequest request)
        {
            Contract.Requires(request.Method == "GET");
            Contract.Requires(request.Headers["Authorization"].StartsWith("Bearer "));  

            return new MemoryStream(Encoding.Default.GetBytes("something"));
        }
    }
}
