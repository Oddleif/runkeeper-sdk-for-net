﻿using Oddleif.RunKeeper.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace Oddleif.RunKeeper.Client.Test
{
    /// <summary>
    ///This is a test class for FitnessActivityTest and is intended
    ///to contain all FitnessActivityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FitnessActivityTest
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        /// Ensures that the heart rates never are initialized to null.
        ///</summary>
        [TestMethod()]
        public void HeartRatesNullTest()
        {
            var target = new FitnessActivity();

            Assert.IsNotNull(target.HeartRates);
            Assert.AreEqual(0, target.HeartRates.Count);
        }

        /// <summary>
        /// Ensures that the heart rates never are initialized to null.
        ///</summary>
        [TestMethod()]
        public void DistanceNullTest()
        {
            var target = new FitnessActivity();

            Assert.IsNotNull(target.Distances);
            Assert.AreEqual(0, target.Distances.Count);
        }

        /// <summary>
        /// Ensures that the heart rates never are initialized to null.
        ///</summary>
        [TestMethod()]
        public void PathNullTest()
        {
            var target = new FitnessActivity();

            Assert.IsNotNull(target.ActivityPath);
            Assert.AreEqual(0, target.ActivityPath.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(RunKeeperClientException))]
        public void ValidationHandlerErrorTest()
        {
            var document = new System.Xml.XmlDocument();
            var rootElement = document.CreateElement("TrainingCenterDatabase", "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");
            document.AppendChild(rootElement);

            rootElement.AppendChild(document.CreateElement("unknown"));

            FitnessActivity.ValidatateTcxXml(document);
        }
    }
}
