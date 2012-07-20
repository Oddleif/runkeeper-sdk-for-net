using System.Configuration;

namespace RunKeeperClientApi
{
    internal static class WebProxyFactory
    {
        static readonly bool _testMode = ConfigurationManager.AppSettings["RunKeeperClientApi.Mode"] == "Test";

        public static RunKeeperWebProxy GetWebProxy()
        {
            if (!_testMode)
                return new RunKeeperWebProxy();
            else
                return new RunKeeperTestProxy();
        }
    }
}
