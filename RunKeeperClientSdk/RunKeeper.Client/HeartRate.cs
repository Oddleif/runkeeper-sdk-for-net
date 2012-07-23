using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RunKeeper.Client
{
    public class HeartRate
    {
        /// <summary>
        /// Timestamp for hear rate measurement.
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
