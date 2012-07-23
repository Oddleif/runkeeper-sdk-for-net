using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;

namespace RunKeeper.Client.Test
{
    /// <summary>    
    /// https://runkeeper.com/apps/authorize?client_id=2083a749bab14979a4fb09ec457d80ae&redirect_uri=http://localhost&response_type=code
    /// {"token_type":"Bearer","access_token":"2ec59fa926d044bea8dc256174619625"}
    /// </summary>
    [TestClass]
    public class RunKeeperAccountTest
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

        [TestMethod]
        public void GetFitnessActivityFeedTest()
        {
            var account = GetActiveRunKeeperAccount();

            var fitnessActivityFeed = account.GetFitnessActivityFeed();

            ValidateFeed(fitnessActivityFeed);

            ValidateFeedItems(fitnessActivityFeed);
        }

        [TestMethod]
        public void Equals1Test()
        {
            Assert.AreEqual(new RunKeeperAccount("a"), new RunKeeperAccount("a"));
        }

        [TestMethod]
        public void Equals2Test()
        {
            Assert.AreNotEqual(new RunKeeperAccount("a"), null);
        }

        [TestMethod]
        public void Equals3Test()
        {
            Assert.AreNotEqual(new RunKeeperAccount("a"), new object());
        }

        [TestMethod]
        public void Equals4Test()
        {
            Assert.AreNotEqual(new RunKeeperAccount("a"), new RunKeeperAccount("b"));
        }

        [TestMethod]
        public void GetHashCodeWithSameAccessTokenTest()
        {
            var feedA = new RunKeeperAccount("a");
            var feedB = new RunKeeperAccount("a");

            Assert.AreNotEqual(feedA.GetHashCode(), feedB.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeWithDifferentAccessTokenTest()
        {
            var feedA = new RunKeeperAccount("a");
            var feedB = new RunKeeperAccount("b");

            Assert.AreNotEqual(feedA.GetHashCode(), feedB.GetHashCode());
        }

        private static void ValidateFeed(FitnessActivityFeed fitnessActivityFeed)
        {
            Assert.IsNotNull(fitnessActivityFeed);
            Assert.IsNull(fitnessActivityFeed.PreviousPageUri);
            Assert.AreEqual(new Uri("/fitnessActivities?page=1&pageSize=2&noEarlierThan=1970-01-01&noLaterThan=2012-07-22&modifiedNoEarlierThan=1970-01-01&modifiedNoLaterThan=2012-07-22", UriKind.Relative), fitnessActivityFeed.NextPageUri);
            Assert.AreEqual(83, fitnessActivityFeed.TotalActivityCount);
            Assert.AreEqual(2, fitnessActivityFeed.Items.Count);
        }

        private static void ValidateFeedItems(FitnessActivityFeed fitnessActivityFeed)
        {
            var runningActivity = fitnessActivityFeed.Items[0];
            Assert.AreEqual("Running", runningActivity.ActivityType);
            Assert.AreEqual(7581.0285921453, runningActivity.Distance);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 2677, 43), runningActivity.Duration);
            Assert.AreEqual(2677.43, runningActivity.DurationInSeconds);
            Assert.AreEqual("Fri, 20 Jul 2012 09:52:29", runningActivity.StartTime);
            Assert.AreEqual(new Uri("/fitnessActivities/103227434", UriKind.Relative), runningActivity.ActivityUri);

            var cyclingActivity = fitnessActivityFeed.Items[1];
            Assert.AreEqual("Cycling", cyclingActivity.ActivityType);
            Assert.AreEqual(46387.3439279308, cyclingActivity.Distance);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 7029, 0), cyclingActivity.Duration);
            Assert.AreEqual(7029, cyclingActivity.DurationInSeconds);
            Assert.AreEqual("Thu, 19 Jul 2012 10:29:09", cyclingActivity.StartTime);
            Assert.AreEqual(new Uri("/fitnessActivities/103032067", UriKind.Relative), cyclingActivity.ActivityUri);
        }

        internal static RunKeeperAccount GetActiveRunKeeperAccount()
        {
            Contract.Ensures(Contract.Result<RunKeeperAccount>() != null);

            return RunKeeperAccountsRepository.GetRunKeeperAccount("Bearer 2ec59fa926d044bea8dc256174619625");
        }
    }
}
