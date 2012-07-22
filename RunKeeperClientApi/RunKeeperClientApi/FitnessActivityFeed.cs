using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RunKeeperClientApi
{
    /// <summary>
    /// A collection of fitness activities. The result source 
    /// is paged, meaning each FitnessActivityFeed object will 
    /// only contain page of activities. To get them all
    /// you will need to continue loading as long as there is a <see cref="Next"/>
    /// endpoitn available.
    /// </summary>
    [DataContract]
    public class FitnessActivityFeed
    {
        /// <summary>
        /// Endpoint address to the previous set of activities in the feed.
        /// </summary>
        [DataMember(Name="previous")]
        public string Previous { get; set; }

        /// <summary>
        /// Endpoint address to the previous set of activities in the feed.
        /// </summary>
        [DataMember(Name="next")]
        public string Next { get; set; }

        /// <summary>
        /// Contains the total number of activities in the feed.
        /// </summary>
        [DataMember(Name="size")]
        public int TotalActivityCount { get; set; }

        /// <summary>
        /// The set of fitness activities in the feed. If the feed contains
        /// multiple pages it will only contain the items for the current page.
        /// </summary>
        [DataMember(Name="items")]
        public IEnumerable<FitnessActivityFeedItem> Items { get; set; }
    }
}
