using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Xml.Schema;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentFolder">Folder where the file should be created.</param>
        /// <returns></returns>
        public string SaveAsTcx(string parentFolder)
        {
            Contract.Requires(Directory.Exists(parentFolder));
            // Assumes it's the same entries in all.
            // what about missing heart rate data?            
            Contract.Requires(ActivityPath.Count == Distances.Count);

            var filename = GetTcxFilename(parentFolder);

            if (File.Exists(filename))
                File.Delete(filename);
            
            var xmlDocument = new XmlDocument();
            
            var rootElement = xmlDocument.CreateElement("TrainingCenterDatabase", "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");
            xmlDocument.AppendChild(rootElement);
            xmlDocument.InsertBefore(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null), rootElement);

            var activities = AddChildeNode(rootElement, "Activities", null);
            AddActivity(activities);

            ValidatateTcxXml(xmlDocument);
            
            xmlDocument.Save(filename);
            
            return filename;
        }

        internal static void ValidatateTcxXml(XmlDocument xmlDocument)
        {
            xmlDocument.Schemas.Add("http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2", "http://www.garmin.com/xmlschemas/TrainingCenterDatabasev2.xsd");
            xmlDocument.Validate(ValidationEventHandler);
        }

        private string GetTcxFilename(string parentFolder)
        {
            var filename = Path.Combine(parentFolder, ActivityUri.OriginalString.Split(new char[] { '/' }).Last() + ".tcx");
            return filename;
        }

        private void AddActivity(XmlNode activities)
        {
            var activity = AddChildeNode(activities, "Activity", null);
            SetSport(activity);
            
            var idNode = AddChildeNode(activity, "Id", StartTime.ToUniversalTime().ToString("u").Replace(' ', 'T'));

            AddLap(activity);
        }

        private void AddLap(XmlNode activity)
        {
            var lap = AddChildeNode(activity, "Lap", null);
            lap.Attributes.Append(lap.OwnerDocument.CreateAttribute("StartTime")).Value = StartTime.ToUniversalTime().ToString("u").Replace(' ', 'T');

            AddChildeNode(lap, "TotalTimeSeconds", this.DurationInSeconds.ToString(CultureInfo.InvariantCulture));
            AddChildeNode(lap, "DistanceMeters", this.Distance.ToString(CultureInfo.InvariantCulture));
            AddChildeNode(lap, "Calories", this.TotalCalories.ToString());
            AddChildeNode(lap, "Intensity", "Active");
            AddChildeNode(lap, "TriggerMethod", "Manual");

            AddTrack(lap);
        }

        private void AddTrack(XmlNode lap)
        {
            Contract.Requires(ActivityPath.Count > 0);

            var track = AddChildeNode(lap, "Track", null);

            for (int i = 0; i < ActivityPath.Count; i++)
                AddTrackpoint(track, i);
        }

        private void SetSport(XmlNode activity)
        {
            var activityType = activity.Attributes.Append(activity.OwnerDocument.CreateAttribute("Sport"));

            if (this.ActivityType == "Cycling")
                activityType.Value = "Biking";
            else if (ActivityType != "Running")
                activityType.Value = "Other";
            else
                activityType.Value = this.ActivityType;
        }

        private void AddTrackpoint(XmlNode track, int i)
        {
            var point = ActivityPath[i];
            var heartRate = HeartRates.Count == 0 ? null : HeartRates.Where(x => x.Timestamp == point.Timestamp).FirstOrDefault();
            var distance = Distances[i];

            if (heartRate != null)
                Contract.Assert(point.Timestamp == heartRate.Timestamp);

            Contract.Assume(point.Timestamp == distance.Timestamp);

            var trackPoint = AddChildeNode(track, "Trackpoint", null);
            AddChildeNode(trackPoint, "Time", StartTime.AddSeconds(point.Timestamp).ToUniversalTime().ToString("u").Replace(' ', 'T'));
            var position = AddChildeNode(trackPoint, "Position", null);
            AddChildeNode(position, "LatitudeDegrees", point.Latitude.ToString(CultureInfo.InvariantCulture));
            AddChildeNode(position, "LongitudeDegrees", point.Longitude.ToString(CultureInfo.InvariantCulture));
            AddChildeNode(trackPoint, "AltitudeMeters", point.Altitude.ToString(CultureInfo.InvariantCulture));
            AddChildeNode(trackPoint, "DistanceMeters", distance.DistanceInMeters.ToString(CultureInfo.InvariantCulture));

            if (heartRate != null)
            {
                var heartRateBpm = AddChildeNode(trackPoint, "HeartRateBpm", null);
                AddChildeNode(heartRateBpm, "Value", heartRate.BeatsPerMinute.ToString(CultureInfo.InvariantCulture));
            }
        }

        private XmlNode AddChildeNode(XmlNode parent, string childName, string innerText)
        {
            var element = parent.OwnerDocument.CreateElement(childName, "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");

            if (!String.IsNullOrEmpty(innerText))
                element.InnerText = innerText;

            return parent.AppendChild(element);
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new Exception(e.Severity.ToString() + ": " + e.Message, e.Exception);            
        }
    }
}
