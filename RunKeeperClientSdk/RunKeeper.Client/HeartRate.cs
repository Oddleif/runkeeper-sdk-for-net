using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RunKeeper.Client
{
    /// <summary>
    /// Contains a hear rate measurement for a given time
    /// in an activity.
    /// </summary>
    [DataContract]
    public class HeartRate
    {
        /// <summary>
        /// Seconds since activity starte when the hear rate measurement
        /// was taken.
        /// </summary>
        [DataMember(Name="timestamp")]
        public double Timestamp { get; set; }
        
        /// <summary>
        /// Beats per minute at the given timestamp.
        /// </summary>
        [DataMember(Name="heart_rate")]
        public int BeatsPerMinute { get; set; }
    }
}
