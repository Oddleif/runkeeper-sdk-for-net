using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace RunKeeperClientApi
{
    /// <summary>
    /// A collection of fitness activities. The result source 
    /// is paged, meaning each FitnessActivityFeed object will 
    /// only contain page of activities. To get them all
    /// you will need to continue loading as long as there is a <see cref="NextPageUri"/>
    /// endpoitn available.
    /// </summary>
    [DataContract]
    public class FitnessActivityFeed
    {
        /// <summary>
        /// Endpoint address to the previous set of activities in the feed.
        /// </summary>
        [DataMember(Name="previous")]
        public Uri PreviousPageUri { get; set; }

        /// <summary>
        /// Endpoint address to the previous set of activities in the feed.
        /// </summary>
        [DataMember(Name="next")]
        public Uri NextPageUri { get; set; }

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
        public IList<FitnessActivityFeedItem> Items { get; set; }

        /// <summary>
        /// Indicates if the current feed has more pages or not.
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return NextPageUri != null;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return PreviousPageUri != null;
            }
        }

        public FitnessActivityFeed GetNextPage()
        {
            Contract.Requires(HasNextPage);

            return RunKeeperAccount.GetFitnessActivityFeed(NextPageUri);
        }

        public FitnessActivityFeed GetPreviousPage()
        {
            Contract.Requires(HasPreviousPage);

            return RunKeeperAccount.GetFitnessActivityFeed(PreviousPageUri);
        }

        internal RunKeeperAccount RunKeeperAccount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is FitnessActivityFeed == false)
                return false;

            var compareTo = (FitnessActivityFeed)obj;

            if (NextPageUri != compareTo.NextPageUri)
                return false;
            if (PreviousPageUri != compareTo.PreviousPageUri)
                return false;
            if (RunKeeperAccount.AccessToken != compareTo.RunKeeperAccount.AccessToken)
                return false;
            if (TotalActivityCount != compareTo.TotalActivityCount)
                return false;
            if (!Items.SequenceEqual(compareTo.Items))
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Not changed implementation according to Equals implementation.
        /// Hence, object instances are not consideres the same - though their content equals.
        /// </remarks>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
