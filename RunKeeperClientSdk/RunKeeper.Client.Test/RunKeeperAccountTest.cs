using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;
using System.Web;
using System.IO;

namespace RunKeeper.Client.Test
{
    /// <summary>    
    /// https://runkeeper.com/apps/authorize?client_id=2083a749bab14979a4fb09ec457d80ae&redirect_uri=http://localhost&response_type=code
    /// {"token_type":"Bearer","access_token":"2ec59fa926d044bea8dc256174619625"}
    /// </summary>
    [TestClass]
    public class RunKeeperAccountTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
            Assert.AreEqual("Fri, 20 Jul 2012 09:52:29", runningActivity.StartTimeString);
            Assert.AreEqual(new Uri("/fitnessActivities/103227434", UriKind.Relative), runningActivity.ActivityUri);

            var cyclingActivity = fitnessActivityFeed.Items[1];
            Assert.AreEqual("Cycling", cyclingActivity.ActivityType);
            Assert.AreEqual(46387.3439279308, cyclingActivity.Distance);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 7029, 0), cyclingActivity.Duration);
            Assert.AreEqual(7029, cyclingActivity.DurationInSeconds);
            Assert.AreEqual("Thu, 19 Jul 2012 10:29:09", cyclingActivity.StartTimeString);
            Assert.AreEqual(new Uri("/fitnessActivities/103032067", UriKind.Relative), cyclingActivity.ActivityUri);
        }

        internal static RunKeeperAccount GetActiveRunKeeperAccount()
        {
            Contract.Ensures(Contract.Result<RunKeeperAccount>() != null);

            return RunKeeperAccount.GetRunKeeperAccount("Bearer e37ea03007e3459eb2bcff30e598c9b8");
        }

        [TestMethod]
        public void GetFitnessActivityTest()
        {
            var account = GetActiveRunKeeperAccount();

            var activity = account.GetFitnessActivity(new Uri("/fitnessActivities/103032067", UriKind.Relative));

            Assert.AreEqual("Cycling", activity.ActivityType);
            Assert.AreEqual(new Uri("/fitnessActivities/103032067", UriKind.Relative), activity.ActivityUri);
            Assert.AreEqual(120, activity.AverageHeartRate);
            Assert.AreEqual(647.363636363638, activity.Climb);
            Assert.AreEqual(46387.3439279308, activity.Distance);
            Assert.AreEqual(7029, activity.DurationInSeconds);
            Assert.AreEqual("None", activity.Equipment);
            
            // Just checking the count including first and last
            // to get an indication about the collection beging valid or not.
            Assert.AreEqual(3430, activity.HeartRates.Count);
            Assert.AreEqual(0, activity.HeartRates[0].Timestamp);
            Assert.AreEqual(84, activity.HeartRates[0].BeatsPerMinute);
            Assert.AreEqual(7029, activity.HeartRates[3429].Timestamp);
            Assert.AreEqual(106, activity.HeartRates[3429].BeatsPerMinute);

            var startPoint = activity.ActivityPath[0];
            Assert.AreEqual(187, startPoint.Altitude);
            Assert.AreEqual(59.881485, startPoint.Latitude);
            Assert.AreEqual(10.849346, startPoint.Longitude);
            Assert.AreEqual("start", startPoint.PointType);
            Assert.AreEqual(0, startPoint.Timestamp);

            var endPoint = activity.ActivityPath[activity.ActivityPath.Count - 1];
            Assert.AreEqual(187, endPoint.Altitude);
            Assert.AreEqual(59.881594, endPoint.Latitude);
            Assert.AreEqual(10.849361, endPoint.Longitude);
            Assert.AreEqual("end", endPoint.PointType);
            Assert.AreEqual(7029, endPoint.Timestamp);

            Assert.AreEqual(0, activity.Distances[0].Timestamp);
            Assert.AreEqual(0, activity.Distances[0].DistanceInMeters);
            Assert.AreEqual(7029, activity.Distances[activity.Distances.Count - 1].Timestamp);
            Assert.AreEqual(46387.733873782054, activity.Distances[activity.Distances.Count - 1].DistanceInMeters);

            Assert.AreEqual(false, activity.IsLive);            
            Assert.AreEqual("Thu, 19 Jul 2012 10:29:09", activity.StartTimeString);
            Assert.AreEqual(1274, activity.TotalCalories);
            Assert.AreEqual(8819026, activity.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpException))]
        public void GetFitnessActiviyInvalidUriTest()
        {
            var account = GetActiveRunKeeperAccount();

            account.GetFitnessActivity(new Uri("/fitnessActivities/516468", UriKind.Relative));
        }

        [TestMethod]
        public void GetRunKeeperProfileTest()
        {
            var account = GetActiveRunKeeperAccount();
            var profile = account.GetProfile();

            Assert.AreEqual("Oddleif Halvorsen", profile.Name);
            Assert.AreEqual("Oslo, Norway", profile.Location);
            Assert.AreEqual(false, profile.Elite);
            Assert.AreEqual("Fri, 11 Sep 1981 00:00:00", profile.BirthdayDateString);
        }

        [TestMethod]
        public void FitnessUriNullTest()
        {
            var account = new RunKeeperAccount("Bearer ...");

            Assert.IsNull(account.FitnessActivitiesUri);
        }

        [TestMethod]
        public void ProfileUriNullTest()
        {
            var account = new RunKeeperAccount("Bearer ...");

            Assert.IsNull(account.ProfileUri);
        }

        [TestMethod]
        public void SaveActivityAsTcxFilenameTest()
        {
            var account = GetActiveRunKeeperAccount();

            var activity = account.GetFitnessActivity(new Uri("/fitnessActivities/103032067", UriKind.Relative));

            var actualFilename = activity.SaveAsTcx(Directory.GetCurrentDirectory());
            var excpectedFilename = Path.Combine(Directory.GetCurrentDirectory(), "103032067.tcx");

            Assert.AreEqual(excpectedFilename, actualFilename);
        }

        [TestMethod]
        public void SaveCyclingActivityWithHeartRateAsTcxFileExistsTest()
        {
            var account = GetActiveRunKeeperAccount();

            var activity = account.GetFitnessActivity(new Uri("/fitnessActivities/103032067", UriKind.Relative));

            var actualFilename = activity.SaveAsTcx(Directory.GetCurrentDirectory());

            Assert.IsTrue(File.Exists(actualFilename));
        }

        [TestMethod]
        public void SaveRunningActivityWithoutHeartRateAsTcxFile()
        {
            var account = GetActiveRunKeeperAccount();
            
            var activity = account.GetFitnessActivity(new Uri("/fitnessActivities/78576346", UriKind.Relative));
            
            activity.SaveAsTcx(Directory.GetCurrentDirectory());

            // If no schema validation error or other error, assume okay for now.
            // TODO: Add verification of expected output.
        }

        [TestMethod]
        public void SaveWalkingActivityWithoutHeartRateAsTcxFile()
        {
            var account = GetActiveRunKeeperAccount();

            var activity = account.GetFitnessActivity(new Uri("/fitnessActivities/90834782", UriKind.Relative));
            
            activity.SaveAsTcx(Directory.GetCurrentDirectory());

            // If no schema validation error or other error, assume okay for now.
            // TODO: Add verification of expected output.
        }
    }
}
