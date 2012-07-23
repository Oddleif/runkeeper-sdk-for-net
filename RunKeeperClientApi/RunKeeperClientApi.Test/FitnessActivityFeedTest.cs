using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace RunKeeperClientApi.Test
{
    [TestClass]
    public class FitnessActivityFeedTest
    {
        [TestMethod]
        public void HasNextPageTest()
        {
            var feed = new FitnessActivityFeed() { NextPageUri = new Uri("/NextPage", UriKind.Relative) };

            Assert.IsTrue(feed.HasNextPage);
        }

        [TestMethod]
        public void DoesNotHaveNextPageTest()
        {
            var feed = new FitnessActivityFeed();

            Assert.IsFalse(feed.HasNextPage);
        }

        [TestMethod]
        public void HasPreviousPageTest()
        {
            var feed = new FitnessActivityFeed() { PreviousPageUri = new Uri("/PreviousPage", UriKind.Relative) };

            Assert.IsTrue(feed.HasPreviousPage);
        }

        [TestMethod]
        public void DoesNotHavePreviousPageTest()
        {
            var feed = new FitnessActivityFeed();

            Assert.IsFalse(feed.HasPreviousPage);
        }

        /// <summary>
        /// No next page exists.
        /// </summary>
        [TestMethod]
        public void GetNonExistingNextPageTest()
        {
            try
            {
                var feed = new FitnessActivityFeed();

                var secondPage = feed.GetNextPage();
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

        /// <summary>
        /// No next page exists.
        /// </summary>
        [TestMethod]
        public void GetNonExistingPreviousPageTest()
        {
            try
            {
                var feed = new FitnessActivityFeed();

                var secondPage = feed.GetPreviousPage();
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
        public void PagingTest()
        {
            var feed = RunKeeperAccountTest.GetActiveRunKeeperAccount().GetFitnessActivityFeed();

            var nextPage = feed.GetNextPage();
            var firstPage = nextPage.GetPreviousPage();

            Assert.IsNotNull(nextPage);
            Assert.IsNotNull(nextPage.PreviousPageUri);
            Assert.IsNotNull(nextPage.NextPageUri);
            Assert.AreEqual(feed, firstPage);
        }

        [TestMethod]
        public void Equals1Test()
        {
            Assert.AreEqual(new FitnessActivityFeed(), new FitnessActivityFeed());
        }

        [TestMethod]
        public void Equals2Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeed(), new Object());
        }

        [TestMethod]
        public void Equals3Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeed() { NextPageUri = new Uri("/some", UriKind.Relative) }, new FitnessActivityFeed() { NextPageUri = new Uri("/other", UriKind.Relative) });
        }

        [TestMethod]
        public void Equals4Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeed() { PreviousPageUri = new Uri("/some", UriKind.Relative) },
                               new FitnessActivityFeed() { PreviousPageUri = new Uri("/other", UriKind.Relative) });
        }

        [TestMethod]
        public void Equals5Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeed() { RunKeeperAccount = new RunKeeperAccount("token") },
                               new FitnessActivityFeed() { RunKeeperAccount = new RunKeeperAccount("other token") });
        }

        [TestMethod]
        public void Equals6Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeed(),
                               new FitnessActivityFeed() { RunKeeperAccount = new RunKeeperAccount("other token") });
        }

        [TestMethod]
        public void Equals7Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeed() { RunKeeperAccount = new RunKeeperAccount("token") },
                               new FitnessActivityFeed() );
        }

        [TestMethod]
        public void Equals8Test()
        {
            Assert.AreNotEqual(new FitnessActivityFeed() { TotalActivityCount = 4 },
                               new FitnessActivityFeed() { TotalActivityCount = 6 });
        }

        [TestMethod]
        public void Equals9Test()
        {
            var feedA = new FitnessActivityFeed() { Items = new List<FitnessActivityFeedItem>() { new FitnessActivityFeedItem() { ActivityType = "Running" } } };
            var feedB = new FitnessActivityFeed() { Items = new List<FitnessActivityFeedItem>() { new FitnessActivityFeedItem() { ActivityType = "Cycling" } } };

            Assert.AreNotEqual(feedA, feedB);
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            var feedA = new FitnessActivityFeed();
            var feedB = new FitnessActivityFeed();

            Assert.AreNotEqual(feedA.GetHashCode(), feedB.GetHashCode());
        }
    }
}
