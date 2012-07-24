using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunKeeper.Client.Test
{
    [TestClass]
    public class HeartRateTest
    {
        [TestMethod]
        public void HeartRatePropertiesTest()
        {
            var hearRate = new HeartRate() { Timestamp = 12, BeatsPerMinute = 120 };

            Assert.AreEqual(12, hearRate.Timestamp);
            Assert.AreEqual(120, hearRate.BeatsPerMinute);
        }
    }
}
