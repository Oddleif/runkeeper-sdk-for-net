using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RunKeeper.Client
{
    [DataContract]
    public struct Distance
    {
        [DataMember(Name="timestamp")]
        public int Timestamp { get; set; }

        [DataMember(Name="distance")]
        public double DistanceInMeters { get; set; }
    }
}
