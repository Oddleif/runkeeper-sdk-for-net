﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace RunKeeper.Client
{
    /// <summary>
    /// Represents a fitness activity.
    /// </summary>
    [DataContract]
    public class FitnessActivity : FitnessActivityFeedItem
    {
        private IList<HeartRate> _heartRates = new List<HeartRate>();
        private IList<Point> _activityPath = new List<Point>();
        private IList<Distance> _distances = new List<Distance>();

        /// <summary>
        /// The activity owner id.
        /// </summary>
        [DataMember(Name="userID")]
        public int UserId { get; set; }

        /// <summary>
        /// Total climt in metric meters.
        /// </summary>
        [DataMember(Name="climb")]
        public double Climb { get; set; }

        [DataMember(Name="equipment")]
        public string Equipment { get; set; }

        /// <summary>
        /// Number of calories burned during the activity.
        /// </summary>
        [DataMember(Name="total_calories")]
        public int TotalCalories { get; set; }

        /// <summary>
        /// Average heart rate for the activity.
        /// </summary>
        [DataMember(Name="average_heart_rate")]
        public int AverageHeartRate { get; set; }

        [DataMember(Name="is_live")]
        public bool IsLive { get; set; }

        /// <summary>
        /// List of heart rate measurements for the activity.
        /// </summary>
        [DataMember(Name="heart_rate")]
        public IList<HeartRate> HeartRates 
        {
            get
            {
                return _heartRates;
            }
            set
            {
                _heartRates = value;
            }
        }

        /// <summary>
        /// The list of points that give the full path for the activity.
        /// </summary>
        [DataMember(Name="path")]
        public IList<Point> ActivityPath
        {
            get
            {
                return _activityPath;
            }
            set
            {
                _activityPath = value;
            }
        }

        /// <summary>
        /// Contains a list of distances for given timestamps.
        /// </summary>
        [DataMember(Name = "distance")]
        public IList<Distance> Distances
        {
            get
            {
                return _distances;
            }
            set
            {
                _distances = value;
            }
        }       
    }
}
