using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oddleif.RunKeeper.Client.Test
{
    [TestClass]
    public class RunKeeperProfileTest
    {
        [TestMethod]
        public void GetBirthdayConverstionTest()
        {
            var dateString = "Fri, 11 Sep 1981 00:00:00";
            var item = new RunKeeperProfile() { BirthdayDateString = "Fri, 11 Sep 1981 00:00:00" };

            // Since runkeeper is not providing us with a timezone I just skip that part
            Assert.IsTrue(item.Birthday.ToString("R").StartsWith(dateString));
        }
    }
}
