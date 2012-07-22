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
    public class FitnessActivityFeedTest
    {
        [TestMethod]
        public void FitnessActivityFeedHasNextPageTest()
        {
            var feed = new FitnessActivityFeed() { NextPageUri = new Uri("/NextPage", UriKind.Relative) };

            Assert.IsTrue(feed.HasNextPage);
        }

        [TestMethod]
        public void FitnessActivityFeedDoesNotHaveNextPageTest()
        {
            var feed = new FitnessActivityFeed();

            Assert.IsFalse(feed.HasNextPage);
        }

        [TestMethod]
        public void FitnessActivityFeedHasPreviousPageTest()
        {
            var feed = new FitnessActivityFeed() { PreviousPageUri = new Uri("/PreviousPage", UriKind.Relative) };

            Assert.IsTrue(feed.HasPreviousPage);
        }

        [TestMethod]
        public void FitnessActivityFeedDoesNotHavePreviousPageTest()
        {
            var feed = new FitnessActivityFeed();

            Assert.IsFalse(feed.HasPreviousPage);
        }
    }
}
