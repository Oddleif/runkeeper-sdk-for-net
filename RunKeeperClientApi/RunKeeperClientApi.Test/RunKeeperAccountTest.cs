using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;

namespace RunKeeperClientApi.Test
{
    /// <summary>    
    /// https://runkeeper.com/apps/authorize?client_id=2083a749bab14979a4fb09ec457d80ae&redirect_uri=http://localhost&response_type=code
    /// {"token_type":"Bearer","access_token":"2ec59fa926d044bea8dc256174619625"}
    /// </summary>
    [TestClass]
    public class RunKeeperClientTest
    {
        [TestMethod]
        public void GetRecentActivitiesTest()
        {
            var account = GetActiveRunKeeperAccount();

            var headers = new NameValueCollection();
            headers.Add("Accept", "application/vnd.com.runkeeper.FitnessActivityFeed+json");

            var response = account.Get("/fitnessActivities", headers);

            // TODO: Validate response content
            Assert.IsTrue(!String.IsNullOrEmpty(response));            
        }

        [TestMethod]
        public void GetWithoutHeadersTest()
        {
            var account = GetActiveRunKeeperAccount();

            var response = account.Get("/fitnessActivities", null);

            // TODO: Validate response content
            Assert.IsTrue(!String.IsNullOrEmpty(response));
        }

        [TestMethod]
        public void GetUsingInvalidEndpointFormatTest()
        {
            try
            {
                var account = GetActiveRunKeeperAccount();

                var response = account.Get("fitnessActivities", null);
                Assert.Fail();
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void GetWithAuthorizatioHeaderTest()
        {
            var account = GetActiveRunKeeperAccount();
            
            var headers = new NameValueCollection();
            headers.Add("Accept", "application/vnd.com.runkeeper.FitnessActivityFeed+json");
            headers.Add("Authorization", "invalid");

            var response = account.Get("/fitnessActivities", headers);

            Assert.IsTrue(!headers["Authorization"].Contains("invalid"));
        }

        private static RunKeeperAccount GetActiveRunKeeperAccount()
        {
            Contract.Ensures(Contract.Result<RunKeeperAccount>() != null);

            return RunKeeperAccountRepository.GetRunKeeperAccount("Bearer 2ec59fa926d044bea8dc256174619625");
        }
    }
}
