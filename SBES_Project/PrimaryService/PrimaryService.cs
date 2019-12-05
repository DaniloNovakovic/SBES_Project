using Common;
using System;

namespace PrimaryService
{
    public class PrimaryService : IPrimaryService
    {
        public void SendAlarm(Alarm alarm)
        {
            // TODO:  smestanje u fajl/bazu uz proveru ovlascenja klijenta
            // TODO:  smestanje u buffer za repliciranje

            Console.WriteLine($"Sending alarm: {Environment.NewLine}\t{nameof(alarm.TimeOfAlarm)}: {alarm.TimeOfAlarm}{Environment.NewLine}\t{nameof(alarm.NamoOfClient)}: {alarm.NamoOfClient}{Environment.NewLine}\t{nameof(alarm.Message)}: {alarm.Message}{Environment.NewLine}\t{nameof(alarm.Risk)}: {alarm.Risk}");
        }
    }
}
