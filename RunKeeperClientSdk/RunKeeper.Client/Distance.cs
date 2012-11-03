using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Oddleif.RunKeeper.Client
{
    [DataContract]
    public class Distance
    {
        [DataMember(Name="timestamp")]
        public double Timestamp { get; set; }

        [DataMember(Name="distance")]
        public double DistanceInMeters { get; set; }
    }
}
