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
    }
}
