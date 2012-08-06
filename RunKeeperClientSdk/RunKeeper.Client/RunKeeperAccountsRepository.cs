using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Globalization;
using System.Collections.Specialized;

namespace RunKeeper.Client
{
    /// <summary>
    /// Responsible for providing RunKeeperAccount objects.
    /// </summary>
    internal static class RunKeeperAccountsRepository
    {
        /// <summary>
        /// Returns a new RunKeeperAccount object with a valid access token embedded.
        /// This will request a new access token for the given clientAuthorizationCode.
        /// </summary>
        /// <param name="clientAuthorizationCode"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="redirectUri"></param>
        /// <returns>A RunKeeperAccount object with a valid access token.</returns>
        public static RunKeeperAccount GetRunKeeperAccount(string clientAuthorizationCode, string clientId, string clientSecret, string redirectUri)
        {
            Contract.Requires(!String.IsNullOrEmpty(clientAuthorizationCode));
            Contract.Requires(!String.IsNullOrEmpty(clientId));
            Contract.Requires(!String.IsNullOrEmpty(clientSecret));
            Contract.Requires(redirectUri != null);
            Contract.Ensures(Contract.Result<RunKeeperAccount>() != null);

            var accessToken = GetAccessToken(clientAuthorizationCode, clientId, clientSecret, redirectUri);

            return GetRunKeeperAccount(accessToken);
        }

        /// <summary>
        /// Use this method if you already have an access token and just need a
        /// RunKeeperAccount object.
        /// </summary>
        /// <param name="accessToken">A valid runkeeper access token.</param>
        /// <returns></returns>
        public static RunKeeperAccount GetRunKeeperAccount(string accessToken)
        {
            Contract.Requires(!String.IsNullOrEmpty(accessToken));
            Contract.Ensures(Contract.Result<RunKeeperAccount>() != null);
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<RunKeeperAccount>().AccessToken));

            var headers = new NameValueCollection();
            headers.Add("Accept", "application/vnd.com.runkeeper.User+json");
            headers.Add("Authorization", accessToken);

            var account = WebProxyFactory.GetWebProxy().Get<RunKeeperAccount>("/user", headers);
            account.AccessToken = accessToken;

            return account;
        }

        private static string GetAccessToken(string clientAuthorizationCode, string clientId, string clientSecret, string redirectUri)
        {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            var body = GetRequestBody(clientAuthorizationCode, clientId, clientSecret, redirectUri);

            using (var accessTokenResponse = WebProxyFactory.GetWebProxy().Post("https://runkeeper.com/apps/token", "application/x-www-form-urlencoded", body))
            {
                return GetAccessTokenFromResponse(accessTokenResponse);
            }
        }

        private static string GetAccessTokenFromResponse(Stream accessTokenResponse)
        {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            var serializer = new DataContractJsonSerializer(typeof(AccessToken));
            var accessToken = (AccessToken)serializer.ReadObject(accessTokenResponse);

            return accessToken.ToString();
        }

        private static string GetRequestBody(string clientAuthorizationCode, string clientId, string clientSecret, string redirectUri)
        {
            return String.Format(CultureInfo.InvariantCulture, "grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}&redirect_uri={3}",
                clientAuthorizationCode, clientId, clientSecret, redirectUri);            
        }

        [DataContract]
        private struct AccessToken
        {
            [DataMember(Name="token_type")]
            public string Type { get; set; }
            [DataMember(Name="access_token")]
            public string Token { get; set; }

            public override string ToString()
            {
                return Type + " " + Token;
            }
        }
    }
}
