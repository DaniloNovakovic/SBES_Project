using System;

namespace Common
{
    public class Alarm
    {
        public TimeSpan TimeOfAlarm { get; set; }

        public string NamoOfClient { get; set; }

        public string Message { get; set; }

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
