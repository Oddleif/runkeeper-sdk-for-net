using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Oddleif.RunKeeper.Client
{
    [DataContract]
    public struct Point
    {
        [DataMember(Name="timestamp")]
        public double Timestamp { get; set; }
        
        [DataMember(Name="altitude")]
        public double Altitude { get; set; }

        [DataMember(Name="latitude")]
        public double Latitude { get; set; }        

        [DataMember(Name="longitude")]
        public double Longitude { get; set; }

        [DataMember(Name="type")]
        public string PointType { get; set; }
    }
}
