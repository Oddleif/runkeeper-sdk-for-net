using System.Configuration;

namespace RunKeeper.Client
{
    internal static class WebProxyFactory
    {
        static readonly bool _testMode = ConfigurationManager.AppSettings["RunKeeper.Client.Mode"] == "Test";

        public static RunKeeperWebProxy GetWebProxy()
        {
            if (!_testMode)
                return new RunKeeperWebProxy();
            else
                return new RunKeeperTestProxy();
        }
    }
}
