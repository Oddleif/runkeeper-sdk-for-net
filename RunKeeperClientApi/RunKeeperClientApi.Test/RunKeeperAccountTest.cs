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
            
            Assert.IsTrue(!String.IsNullOrEmpty(response));
            Assert.AreEqual("{\"items\": [{\"duration\":2677.43,\"total_distance\":7581.0285921453,\"start_time\":\"Fri, 20 Jul 2012 09:52:29\",\"type\":\"Running\",\"uri\":\"/fitnessActivities/103227434\"},{\"duration\":7029,\"total_distance\":46387.3439279308,\"start_time\":\"Thu, 19 Jul 2012 10:29:09\",\"type\":\"Cycling\",\"uri\":\"/fitnessActivities/103032067\"}],\"next\": \"/fitnessActivities?page=1&pageSize=2&noEarlierThan=1970-01-01&noLaterThan=2012-07-22&modifiedNoEarlierThan=1970-01-01&modifiedNoLaterThan=2012-07-22\",\"size\": 83,}", response);
        }

        [TestMethod]
        public void GetWithoutHeadersTest()
        {
            var account = GetActiveRunKeeperAccount();
            
            try
            {
                var response = account.Get("/fitnessActivities", null);
                Assert.Fail();
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch
            {
                Assert.IsTrue(true);
            }
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
            catch (AssertFailedException)
            {
                throw;
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

            return RunKeeperAccountsRepository.GetRunKeeperAccount("Bearer 2ec59fa926d044bea8dc256174619625");
        }
    }
}
