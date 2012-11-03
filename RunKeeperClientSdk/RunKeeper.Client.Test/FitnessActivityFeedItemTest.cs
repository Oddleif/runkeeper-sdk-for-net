using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Oddleif.RunKeeper.Client.Test
{
    [TestClass]
    public class FitnessActivityFeedItemTest
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
        public void GetHashCodeTest()
        {
            var feedA = new FitnessActivityFeedItem();
            var feedB = new FitnessActivityFeedItem();

            Assert.AreNotEqual(feedA.GetHashCode(), feedB.GetHashCode());
        }

        [TestMethod]
        public void Equals1Test()
        {
            Assert.AreEqual(new FitnessActivityFeedItem(), new FitnessActivityFeedItem());
        }

        [TestMethod]
        public void Equals2Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeedItem(), null);
        }

        [TestMethod]
        public void Equals3Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeedItem(), new object());
        }

        [TestMethod]
        public void Equals4Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeedItem() { ActivityType = "Running" }, new FitnessActivityFeedItem() { ActivityType = "Cycling" });
        }

        [TestMethod]
        public void Equals5Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeedItem() { ActivityUri = new Uri("/running", UriKind.Relative) }, new FitnessActivityFeedItem() { ActivityUri = new Uri("/cycling", UriKind.Relative) });
        }

        [TestMethod]
        public void Equals6Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeedItem() { Distance = 1234.56 }, new FitnessActivityFeedItem() { Distance = 6543.21  });
        }

        [TestMethod]
        public void Equals7Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeedItem() { DurationInSeconds = 54 }, new FitnessActivityFeedItem() { DurationInSeconds = 135 });
        }

        [TestMethod]
        public void Equals8Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeedItem() { StartTimeString = "Fri, 20 Jul 2012 09:52:29" }, new FitnessActivityFeedItem() { StartTimeString = "Fri, 20 Jul 2012 10:52:29" });
        }

        [TestMethod]        
        public void DurationInSecondsTooManyDigitsTest()
        {
            var feed = new FitnessActivityFeedItem() { DurationInSeconds = 23.1234 };

            Assert.AreEqual(23, feed.Duration.Seconds);
            Assert.AreEqual(123, feed.Duration.Milliseconds);
        }

        [TestMethod]
        public void DurationInSecondsTooManyDigitsTest2()
        {
            var feed = new FitnessActivityFeedItem() { DurationInSeconds = 23.1235 };

            Assert.AreEqual(23, feed.Duration.Seconds);
            Assert.AreEqual(124, feed.Duration.Milliseconds);
        }

        [TestMethod]
        public void DurationInSecondsTest()
        {
            var durationInSeconds = 23.123;
            var item = new FitnessActivityFeedItem() { DurationInSeconds = durationInSeconds };

            Assert.AreEqual(durationInSeconds, item.DurationInSeconds);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 23, 123), item.Duration);
        }

        [TestMethod]
        public void StartTimeDateObjTest()
        {
            var dateString = "Thu, 19 Jul 2012 10:29:09";
            var item = new FitnessActivityFeedItem() { StartTimeString = "Thu, 19 Jul 2012 10:29:09" };

            // Since runkeeper is not providing us with a timezone I just skip that part
            Assert.IsTrue(item.StartTime.ToString("R").StartsWith(dateString));
        }
    }
}
