﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunKeeperClientApi.Test
{
    [TestClass]
    public class RunKeeperRepositoryTest
    {
        [TestMethod]
        public void GetRunKeeperAccountTest()
        {
            var account = RunKeeperAccountRepository.GetRunKeeperAccount("46f07c8ffc1c4ffc935b36ccd8699905", "2083a749bab14979a4fb09ec457d80ae", "55b71988d86b4bbb90166a896499b7e0", "http://localhost");

            Assert.IsNotNull(account);
            Assert.IsTrue(!String.IsNullOrEmpty(account.AccessToken));
        }

        [TestMethod]
        public void GetRunKeeperAccountWithExistingAccessTokenTest()
        {
            var accessToken = "access token";
            var account = RunKeeperAccountRepository.GetRunKeeperAccount(accessToken);

            Assert.AreEqual(accessToken, account.AccessToken);
        }

        [TestMethod]
        public void GetRunKeeperAccountWithEmptyAccessTokenTest()
        {

            try
            {
                var account = RunKeeperAccountRepository.GetRunKeeperAccount("token");

                Assert.Fail();
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void GetAccessTokenMissingParams1Test()
        {
            RunAccessTokenParamsTest("7ad4f4a2109a42729f22c660fe851d48", "clientId", "clientSecret", null);
        }

        private static void RunAccessTokenParamsTest(string clientAuthorizatioCode, string clientId, string clientSecret, string redirectUri)
        {
            try
            {
                var token = RunKeeperAccountRepository.GetRunKeeperAccount(clientAuthorizatioCode, clientId, clientSecret, redirectUri);

                Assert.Fail();
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void GetAccessTokenMissingParams2Test()
        {
            RunAccessTokenParamsTest("clientAuthorizatioCode", "clientId", null, null);
        }

        [TestMethod]
        public void GetAccessTokenMissingParams3Test()
        {
            RunAccessTokenParamsTest("clientAuthorizatioCode", null, null, null);
        }

        [TestMethod]
        public void GetAccessTokenMissingParams4Test()
        {
            RunAccessTokenParamsTest(null, null, null, null);
        }

    }
}