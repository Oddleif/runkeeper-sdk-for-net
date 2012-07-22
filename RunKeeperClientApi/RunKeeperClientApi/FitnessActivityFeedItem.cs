using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
using System.Diagnostics.Contracts;

namespace RunKeeperClientApi
{
    [DataContract]
    public class FitnessActivityFeedItem
    {
        /// <summary>
        /// Total duration in seconds as returned by the 
        /// activity feed.
        /// </summary>
        [DataMember(Name="duration")]
        public double DurationInSeconds { get; set; }

        /// <summary>
        /// Duration in seconds converted into a timespan object
        /// for ease of use.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                var split = DurationInSeconds.ToString(CultureInfo.InvariantCulture).Split('.');
                
                var seconds = Convert.ToInt32(split[0]);
                var ms = split.Length == 1 ? 0: GetMsFromString(split[1]);

                return new TimeSpan(0, 0, 0, seconds, ms);
            }
        }

        private static int GetMsFromString(string msString)
        {
            Contract.Requires(!String.IsNullOrEmpty(msString));
            // 1 second == 1000ms, hence we get up to 3 digits.
            Contract.Requires(msString.Length <= 3);

            return Convert.ToInt32(msString);
        }

        /// <summary>
        /// Total distance in metric meters.
        /// </summary>
        [DataMember(Name = "total_distance")]
        public double Distance { get; set; }

        [DataMember(Name = "type")]
        public string ActivityType { get; set; }

        [DataMember(Name = "start_time")]
        public string StartTime { get; set; }
        
        [DataMember(Name = "uri")]
        public string Endpoint { get; set; }
    }
}
