using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;

namespace RunKeeperClientApi
{
    internal class RunKeeperTestProxy : RunKeeperWebProxy
    {
        public override string Get(Uri url, NameValueCollection headers)
        {
            return "something";
        }

        public override Stream Post(string url, string contentType, string body)
        {            
            return new MemoryStream(Encoding.Default.GetBytes("{\"token_type\":\"Bearer\",\"access_token\":\"2ec59fa926d044bea8dc256174619625\"}"));
        }
    }
}
