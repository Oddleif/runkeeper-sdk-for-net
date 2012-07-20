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

            SetAuthorizationHeader(headers);

            return WebProxyFactory.GetWebProxy().Get(url, headers);
        }

        private void SetAuthorizationHeader(NameValueCollection headers)
        {
            if (!String.IsNullOrEmpty(headers["Authorization"]))
                headers.Set("Authorization", AccessToken);
            else
                headers.Add("Authorization", AccessToken);
        }
    }
}
