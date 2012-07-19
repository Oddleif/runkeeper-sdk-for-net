using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;

namespace RunKeeperClientApi
{
    public class RunKeeperAccount
    {
        public string AccessToken { get; private set; }

        internal RunKeeperAccount(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string Get(Uri url, NameValueCollection headers)
        {
            Contract.Requires(url != null);

            if (headers == null)
                headers = new NameValueCollection();
            
            headers.Add("Authorization", AccessToken);

            return WebProxyFactory.GetWebProxy().Get(url, headers);
        }
    }
}
