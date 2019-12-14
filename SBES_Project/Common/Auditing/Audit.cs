using System;
using System.Diagnostics;

namespace Common.Auditing
{
    public static class Audit
    {
        public static void ReplicationSuccess(Alarm alarm)
        {
            using (var customLog = EventLogFactory.CreateNew())
            {
                string message = string.Format(AuditEvents.AlarmReplicationSuccess, alarm);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
        }

        public static void ReplicationFailure(Alarm alarm, string reason = "")
        {
            using (var customLog = EventLogFactory.CreateNew())
            {
                string message = string.Format(AuditEvents.AlarmReplicationFailure, alarm, reason);
                customLog.WriteEntry(message, EventLogEntryType.Error);
            }
        }

        public static void ReplicationInitiated()
        {
            using (var customLog = EventLogFactory.CreateNew())
            {
                string message = string.Format(AuditEvents.AlarmReplicationInitiated, DateTime.Now);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
        }
    }
}