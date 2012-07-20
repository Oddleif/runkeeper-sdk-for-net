using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Net;

namespace RunKeeperClientApi
{
    /// <summary>
    /// Test proxy to simulate runkeeper api invokations.
    /// Also responsible for validating the actually http request objects.
    /// </summary>
    internal class RunKeeperTestProxy : RunKeeperWebProxy
    {   
        protected override Stream GetResponse(System.Net.HttpWebRequest requestObject)
        {
            return new MemoryStream(Encoding.Default.GetBytes("{\"token_type\":\"Bearer\",\"access_token\":\"2ec59fa926d044bea8dc256174619625\"}"));
        }

        protected override System.Net.HttpWebRequest GetPostRequest(string url, string contentType, string body)
        {
            Contract.Ensures(Contract.Result<HttpWebRequest>().ContentType == contentType);
            Contract.Ensures(Contract.Result<HttpWebRequest>().Method == "POST");
            // TODO: Ensure body is written to request body

            return base.GetPostRequest(url, contentType, body);
        }

        protected override Stream GetResponseStream(HttpWebRequest request)
        {
            Contract.Requires(request.Method == "GET");
            Contract.Requires(request.Headers["Authorization"].StartsWith("Bearer "));  

            return new MemoryStream(Encoding.Default.GetBytes("{\"items\": [{\"duration\":2677.43,\"total_distance\":7581.0285921453,\"start_time\":\"Fri, 20 Jul 2012 09:52:29\",\"type\":\"Running\",\"uri\":\"/fitnessActivities/103227434\"},{\"duration\":7029,\"total_distance\":46387.3439279308,\"start_time\":\"Thu, 19 Jul 2012 10:29:09\",\"type\":\"Cycling\",\"uri\":\"/fitnessActivities/103032067\"}],\"next\": \"/fitnessActivities?page=1&pageSize=2&noEarlierThan=1970-01-01&noLaterThan=2012-07-22&modifiedNoEarlierThan=1970-01-01&modifiedNoLaterThan=2012-07-22\",\"size\": 83,}"));
        }
    }
}
