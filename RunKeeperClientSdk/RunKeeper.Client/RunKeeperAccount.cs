using System;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Oddleif.RunKeeper.Client
{
    /// <summary>
    /// Class to interact with the content related to a
    /// RunKeeper account.
    /// </summary>
    [DataContract]
    public class RunKeeperAccount
    {
        /// <summary>
        /// The access token for the current account.
        /// </summary>
        public string AccessToken { get; internal set; }

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
            Contract.Requires(headers != null && headers.Count > 0);

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
            return GetFitnessActivityFeed(FitnessActivitiesUri);
        }        

        internal FitnessActivityFeed GetFitnessActivityFeed(Uri feedUri)
        {
            Contract.Requires(feedUri != null);
            Contract.Requires(!String.IsNullOrEmpty(feedUri.ToString()));

            var headers = new NameValueCollection();
            headers.Add("Accept", "application/vnd.com.runkeeper.FitnessActivityFeed+json");
            SetAuthorizationHeader(headers);

            var feed = WebProxyFactory.GetWebProxy().Get<FitnessActivityFeed>(feedUri.ToString(), headers);
            feed.RunKeeperAccount = this;

            return feed;
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
            var compareTo = obj as RunKeeperAccount;

            if (compareTo == null)
                return false;
            if (AccessToken != compareTo.AccessToken)
                return false;           

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the fitness activity details for the given uri.
        /// </summary>
        /// <param name="activityUri">The uri to the activity details. For example: /fitnessActivity/123456</param>
        /// <returns>The fitness activity details.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public FitnessActivity GetFitnessActivity(Uri activityUri)
        {
            Contract.Requires(!String.IsNullOrEmpty(activityUri.ToString()));

            var headers = new NameValueCollection();
            headers.Add("Accept", "application/vnd.com.runkeeper.FitnessActivity+json");
            SetAuthorizationHeader(headers);

            return WebProxyFactory.GetWebProxy().Get<FitnessActivity>(activityUri.ToString(), headers);
        }

        /// <summary>
        /// Returns the profile associated with the current RunKeeper account.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public RunKeeperProfile GetProfile()
        {
            var headers = new NameValueCollection();
            headers.Add("Accept", "application/vnd.com.runkeeper.Profile+json");
            SetAuthorizationHeader(headers);

            return WebProxyFactory.GetWebProxy().Get<RunKeeperProfile>(_profileUri, headers);
        }

        [DataMember(Name="userID")]
        public int UserId { get; set; }

        [DataMember(Name = "fitness_activities")]
        private string _fitnessActivitiesUri = null;
        
        [DataMember(Name = "profile")]
        private string _profileUri = null;

        public Uri FitnessActivitiesUri 
        {
            get
            {
                if (String.IsNullOrEmpty(_fitnessActivitiesUri))
                    return null;

                return new Uri(_fitnessActivitiesUri, UriKind.Relative);
            }
        }

        public Uri ProfileUri
        {
            get
            {
                if (String.IsNullOrEmpty(_profileUri))
                    return null;

                return new Uri(_profileUri, UriKind.Relative);
            }
        }

        /// <summary>
        /// Use this method if you already have an access token and just need a
        /// RunKeeperAccount object.
        /// </summary>
        /// <param name="accessToken">A valid runkeeper access token.</param>
        /// <returns></returns>
        public static RunKeeperAccount GetRunKeeperAccount(string accessToken)
        {
            return RunKeeperAccountsRepository.GetRunKeeperAccount(accessToken);
        }

        /// <summary>
        /// Returns a new RunKeeperAccount object with a valid access token embedded.
        /// This will request a new access token for the given clientAuthorizationCode.
        /// </summary>
        /// <param name="clientAuthorizationCode"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="redirectUri"></param>
        /// <returns>A RunKeeperAccount object with a valid access token.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "3#")]
        public static RunKeeperAccount GetRunKeeperAccount(string clientAuthorizationCode, string clientId, string clientSecret, string redirectUri)
        {
            return RunKeeperAccountsRepository.GetRunKeeperAccount(clientAuthorizationCode, clientId, clientSecret, redirectUri);
        }
    }
}
