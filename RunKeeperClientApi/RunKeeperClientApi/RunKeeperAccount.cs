using System;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;

namespace RunKeeperClientApi
{
    /// <summary>
    /// Class to interact with the content related to a
    /// RunKeeper account.
    /// </summary>
    public class RunKeeperAccount
    {
        /// <summary>
        /// The access token for the current
        /// account.
        /// </summary>
        public string AccessToken { get; private set; }

        internal RunKeeperAccount(string accessToken)
        {
            AccessToken = accessToken;
        }

        /// <summary>
        /// A generic method for running get requests on 
        /// runkeeper endpoints with a set of headers.
        /// The Authorizatio header is automatically set based
        /// on the AccessToken property.
        /// </summary>
        /// <param name="url">The RunKeeper endpoint to run GET on.</param>
        /// <param name="headers">List of headers to attach to the GET request.</param>
        /// <returns>The response body.</returns>
        public string Get(string endpoint, NameValueCollection headers)
        {
            Contract.Requires(!String.IsNullOrEmpty(endpoint));

            if (headers == null)
                headers = new NameValueCollection();

            SetAuthorizationHeader(headers);

            return WebProxyFactory.GetWebProxy().Get(endpoint, headers);
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
