using System;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Alarm
    {
        [DataMember]
        public TimeSpan TimeOfAlarm { get; set; }

        [DataMember]
        public string NamoOfClient { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int Risk { get; set; }

        public Alarm(TimeSpan timeOfAlarm, string message) : this()
        {
            TimeOfAlarm = timeOfAlarm;
            NamoOfClient = string.Empty;
            Message = message;
        }

        public Alarm()
        {
            Risk = CalculateRisk();
        }

        public override string ToString()
        {
            return $"{Environment.NewLine}\t{nameof(TimeOfAlarm)}: {TimeOfAlarm}" +
            $"{Environment.NewLine}\t{nameof(NamoOfClient)}: {NamoOfClient}" +
            $"{Environment.NewLine}\t{nameof(Message)}: {Message}" +
            $"{Environment.NewLine}\t{nameof(Risk)}: {Risk}";
        }

        private int CalculateRisk()
        {
            return new Random().Next(1, 101);
        }
    }
}