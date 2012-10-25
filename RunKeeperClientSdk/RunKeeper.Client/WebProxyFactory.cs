using System.Configuration;

namespace Oddleif.RunKeeper.Client
{
    internal static class WebProxyFactory
    {
        static readonly bool _testMode = ConfigurationManager.AppSettings["Oddleif.RunKeeper.Client.Mode"] == "Test";

        public static RunKeeperWebProxy GetWebProxy()
        {
            if (!_testMode)
                return new RunKeeperWebProxy();
            else
                return new RunKeeperTestProxy();
        }
    }
}
