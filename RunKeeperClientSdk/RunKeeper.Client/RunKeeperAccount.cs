using System;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;

namespace RunKeeper.Client
{
    /// <summary>
    /// Class to interact with the content related to a
    /// RunKeeper account.
    /// </summary>
    public class RunKeeperAccount
    {
        /// <summary>
        /// The access token for the current account.
        /// </summary>
        public string AccessToken { get; private set; }

        internal RunKeeperAccount(string accessToken)
        {
            Contract.Requires(!String.IsNullOrEmpty(accessToken));
            Contract.Ensures(!String.IsNullOrEmpty(AccessToken));

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
        /// <remarks>
        /// Consider this an option only for special circumstances where
        /// the other methods don't currently provide you with what's
        /// needed.
        /// </remarks>
        public string Get(string endpoint, NameValueCollection headers)
        {
            Contract.Requires(!String.IsNullOrEmpty(endpoint));
            Contract.Requires(headers != null);
            Contract.Requires(headers.Count > 0);

            SetAuthorizationHeader(headers);

            using (var result = new StreamReader(WebProxyFactory.GetWebProxy().Get(endpoint, headers)))
            {
                return result.ReadToEnd();
            }
        }

        /// <summary>
        /// Retreives a the first page of the finess activity feed for the account.
        /// </summary>
        /// <returns>The first page of the FitnessActivityFeed.</returns>
        public FitnessActivityFeed GetFitnessActivityFeed()
        {
            return GetFitnessActivityFeed(new Uri("/fitnessActivities", UriKind.Relative));
        }        

        internal FitnessActivityFeed GetFitnessActivityFeed(Uri feedUri)
        {
            Contract.Requires(feedUri != null);
            Contract.Requires(!String.IsNullOrEmpty(feedUri.ToString()));

            var responseStream = GetActivityFeedResponseStream(feedUri);

            return GetActivityFeedFromStream(responseStream);
        }

        private FitnessActivityFeed GetActivityFeedFromStream(Stream responseStream)
        {
            Contract.Ensures(Contract.Result<FitnessActivityFeed>().RunKeeperAccount == this);

            var serializer = new DataContractJsonSerializer(typeof(FitnessActivityFeed));

            var feed = (FitnessActivityFeed)serializer.ReadObject(responseStream);

            feed.RunKeeperAccount = this;
            
            return feed;
        }

        private Stream GetActivityFeedResponseStream(Uri feedUri)
        {
            var headers = new NameValueCollection();
            headers.Add("Accept", "application/vnd.com.runkeeper.FitnessActivityFeed+json");
            SetAuthorizationHeader(headers);

            return WebProxyFactory.GetWebProxy().Get(feedUri.ToString(), headers);
        }

        private void SetAuthorizationHeader(NameValueCollection headers)
        {
            Contract.Requires(headers != null);

            if (!String.IsNullOrEmpty(headers["Authorization"]))
                headers.Set("Authorization", AccessToken);
            else
                headers.Add("Authorization", AccessToken);
        }

        public override bool Equals(object obj)
        {
            if (obj is RunKeeperAccount == false)
                return false;

            var compareTo = (RunKeeperAccount)obj;

            if (AccessToken != compareTo.AccessToken)
                return false;           

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public FitnessActivity GetFitnessActivity(Uri activityUri)
        {
            Contract.Requires(!String.IsNullOrEmpty(activityUri.ToString()));

            var headers = new NameValueCollection();
            headers.Add("Accept", "application/vnd.com.runkeeper.FitnessActivity+json");
            SetAuthorizationHeader(headers);

            using (var reponse = WebProxyFactory.GetWebProxy().Get(activityUri.ToString(), headers))
            {
                var serializer = new DataContractJsonSerializer(typeof(FitnessActivity));

                return (FitnessActivity)serializer.ReadObject(reponse);
            }
        }
    }
}
