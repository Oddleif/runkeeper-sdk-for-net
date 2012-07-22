using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;

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
    }
}
