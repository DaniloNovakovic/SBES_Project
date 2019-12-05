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

        public Alarm(TimeSpan timeOfAlarm, string namoOfClient, string message)
        {
            TimeOfAlarm = timeOfAlarm;
            NamoOfClient = namoOfClient;
            Message = message;
            Risk = CalculateRisk();
        }

        private int CalculateRisk()
        {
            return new Random().Next(1, 11);
        }
    }
}
