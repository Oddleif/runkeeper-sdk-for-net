using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunKeeper.Client.Test
{
    [TestClass]
    public class RunKeeperRepositoryTest
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
        public void GetRunKeeperAccountTest()
        {
            var account = RunKeeperAccountsRepository.GetRunKeeperAccount("46f07c8ffc1c4ffc935b36ccd8699905", "2083a749bab14979a4fb09ec457d80ae", "55b71988d86b4bbb90166a896499b7e0", "http://localhost");
            
            Assert.IsNotNull(account);
            Assert.IsTrue(!String.IsNullOrEmpty(account.AccessToken));
            Assert.IsNotNull(account.FitnessActivitiesUri);
            Assert.IsNotNull(account.ProfileUri);
            Assert.AreEqual(8819026, account.UserId);
        }

        [TestMethod]
        public void GetRunKeeperAccountWithExistingAccessTokenTest()
        {
            var accessToken = "Bearer unikeTokenId";
            var account = RunKeeperAccountsRepository.GetRunKeeperAccount(accessToken);

            Assert.AreEqual(accessToken, account.AccessToken);
        }

        [TestMethod]
        public void GetRunKeeperAccountWithEmptyAccessTokenTest()
        {
            try
            {
                var account = RunKeeperAccountsRepository.GetRunKeeperAccount(null);

                Assert.Fail();
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
        public void GetAccessTokenMissingParams1Test()
        {
            RunAccessTokenParamsTest("7ad4f4a2109a42729f22c660fe851d48", "clientId", "clientSecret", null);
        }

        [TestMethod]
        public void GetAccessTokenMissingParams2Test()
        {
            RunAccessTokenParamsTest("clientAuthorizatioCode", "clientId", null, "http://localhost");
        }

        [TestMethod]
        public void GetAccessTokenMissingParams3Test()
        {
            RunAccessTokenParamsTest("clientAuthorizatioCode", null, "clientSecret", "http://localhost");
        }

        [TestMethod]
        public void GetAccessTokenMissingParams4Test()
        {
            RunAccessTokenParamsTest(null, "clientId", "clientSecret", "http://localhost");
        }

        private static void RunAccessTokenParamsTest(string clientAuthorizatioCode, string clientId, string clientSecret, string redirectUri)
        {
            try
            {
                var token = RunKeeperAccountsRepository.GetRunKeeperAccount(clientAuthorizatioCode, clientId, clientSecret, redirectUri);

                Assert.Fail();
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
    }
}
