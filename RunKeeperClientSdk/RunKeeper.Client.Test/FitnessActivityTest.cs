﻿using RunKeeper.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RunKeeper.Client.Test
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
        ///A test for HearRates
        ///</summary>
        [TestMethod()]
        public void HearRatesNullTest()
        {
            FitnessActivity target = new FitnessActivity(); // TODO: Initialize to an appropriate value

            Assert.IsNotNull(target.HearRates);
            Assert.AreEqual(0, target.HearRates.Count);
        }
    }
}