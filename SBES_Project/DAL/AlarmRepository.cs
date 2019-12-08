using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace DAL
{
    public class AlarmRepository : IAlarmRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public AlarmRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(string serviceId, Alarm alarm)
        {
            _context.Alarms.Add(new AlarmEntity(serviceId, alarm));
            _context.SaveChanges();
        }

        public void DeleteAll()
        {
            var alarms = _context.Alarms.ToList();
            _context.Alarms.RemoveRange(alarms);
            _context.SaveChanges();
        }

        public void DeleteAllByClientName(string clientName)
        {
            var alarmsByClientName = _context.Alarms
                .Where(a => a.ClientName.Equals(clientName, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
            _context.Alarms.RemoveRange(alarmsByClientName);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<Alarm> GetAll()
        {
            return _context.Alarms.Select(a => new Alarm() { Message = a.Message, NamoOfClient = a.ClientName, Risk = a.Risk, TimeOfAlarm = a.TimeOfAlarm }).ToList();
        }
    }
}