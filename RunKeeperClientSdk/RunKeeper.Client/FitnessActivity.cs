using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RunKeeper.Client
{
    [DataContract]
    public class FitnessActivity : FitnessActivityFeedItem
    {
        [DataMember(Name="climb")]
        public double Climb { get; set; }

        [DataMember(Name="equipment")]
        public string Equipment { get; set; }

        [DataMember(Name="total_calories")]
        public int TotalCalories { get; set; }

        [DataMember(Name="average_heart_rate")]
        public int AverageHeartRate { get; set; }

        [DataMember(Name="is_live")]
        public bool IsLive { get; set; }

        [DataMember(Name="notes")]
        public string Notes { get; set; }

        [DataMember(Name="heart_rate")]
        public IList<HeartRate> HearRates { get; set; }
    }
}
