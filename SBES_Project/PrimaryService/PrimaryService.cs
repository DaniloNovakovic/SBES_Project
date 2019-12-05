using Common;
using System;

namespace PrimaryService
{
    public class PrimaryService : IPrimaryService
    {
        public void SendAlarm(Alarm alarm)
        {
            // TODO: smestanje u fajl/bazu uz proveru ovlascenja klijenta
            // TODO: smestanje u buffer za repliciranje

            Console.WriteLine($"Sending alarm: {alarm.Message}");
        }
    }
}
