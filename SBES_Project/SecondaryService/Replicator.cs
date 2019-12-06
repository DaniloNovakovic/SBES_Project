using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryService
{
    public class Replicator : IReplicator
    {
        public void SendAlarm(Alarm alarm)
        {
            // TODO: smestanje u bazu na sekundarnom

            Console.WriteLine($"Sending alarm: {Environment.NewLine}\t{nameof(alarm.TimeOfAlarm)}: {alarm.TimeOfAlarm}{Environment.NewLine}\t{nameof(alarm.NamoOfClient)}: {alarm.NamoOfClient}{Environment.NewLine}\t{nameof(alarm.Message)}: {alarm.Message}{Environment.NewLine}\t{nameof(alarm.Risk)}: {alarm.Risk}");
        }
    }
}
