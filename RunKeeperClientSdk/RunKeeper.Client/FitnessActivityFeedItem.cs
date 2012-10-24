using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
using System.Diagnostics.Contracts;

namespace RunKeeper.Client
{
    /// <summary>
    /// Contains high level details about the fitness activity.
    /// It also contains the information required to get the
    /// activity details.
    /// </summary>
    [DataContract]
    public class FitnessActivityFeedItem
    {
        private double _durationInSeconds;

        /// <summary>
        /// Total duration in seconds as returned by the 
        /// activity feed.
        /// </summary>
        [DataMember(Name="duration")]
        internal double DurationInSeconds 
        { 
            get
            {
                return _durationInSeconds;
            }
            
            set 
            {
                _durationInSeconds = Math.Round(value, 3);
            }
        }

        /// <summary>
        /// Duration in seconds converted into a timespan object
        /// for ease of use.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {               
                var split = DurationInSeconds.ToString(CultureInfo.InvariantCulture).Split('.');
                
                var seconds = Convert.ToInt32(split[0], CultureInfo.CurrentCulture);
                var ms = split.Length == 1 ? 0: GetMsFromString(split[1]);

                return new TimeSpan(0, 0, 0, seconds, ms);
            }
        }

        private static int GetMsFromString(string msString)
        {
            Contract.Requires(!String.IsNullOrEmpty(msString));
            // 1 second == 1000ms, hence we get up to 3 digits.
            Contract.Requires(msString.Length <= 3);

            return Convert.ToInt32(msString, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Total distance in metric meters.
        /// </summary>
        [DataMember(Name = "total_distance")]
        public double Distance { get; set; }

        /// <summary>
        /// The type of activity. Running, Cycling, Walking, etc..
        /// </summary>
        [DataMember(Name = "type")]
        public string ActivityType { get; set; }

        /// <summary>
        /// Time when activity started.
        /// </summary>
        [DataMember(Name = "start_time")]
        internal string StartTimeString;

        /// <summary>
        /// Returns the start time for the activity. Runkeeper
        /// does not keep track of timezones, so assume this to be the local time
        /// for wherever the activity was tracked.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                var date = Convert.ToDateTime(StartTimeString, CultureInfo.InvariantCulture);

                return date;
            }
        }
        /// <summary>
        /// The address to where you can get the activity details.
        /// </summary>
        [DataMember(Name = "uri")]
        public Uri ActivityUri { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is FitnessActivityFeedItem == false)
                return false;

            var compareTo = (FitnessActivityFeedItem)obj;

            if (this.ActivityType != compareTo.ActivityType)
                return false;
            if (this.ActivityUri != compareTo.ActivityUri)
                return false;
            if (this.Distance != compareTo.Distance)
                return false;
            if (DurationInSeconds != compareTo.DurationInSeconds)
                return false;
            if (this.StartTimeString != compareTo.StartTimeString)
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Not changed according to equals. Still consider object instance different - even 
        /// though their content are the same.
        /// </remarks>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
