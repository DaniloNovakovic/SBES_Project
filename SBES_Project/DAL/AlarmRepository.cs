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

        #region Alarm
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
                .Where(a => a.ClientName.Equals(clientName, StringComparison.OrdinalIgnoreCase))
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
        #endregion

        #region ClientRequests
        public void AddClientRequest(string clientName)
        {
            if (_context.ClientRequests.Select(s=> s.ClientName).Where(item=> item.Equals(clientName, StringComparison.OrdinalIgnoreCase)).Count() < 1)
            {
                _context.ClientRequests.Add(new ClientRequestsEntity() { ClientName = clientName });
                _context.SaveChanges();
            }
        }

        public void RemoveClientRequest(string clientName)
        {
            var client = _context.ClientRequests.Where(item => item.ClientName.Equals(clientName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            _context.ClientRequests.Remove(client);
            _context.SaveChanges();
        }

        public IEnumerable<string> GetAllClientRequests()
        {
            return _context.ClientRequests.Select(s => s.ClientName).ToList();
        }
        #endregion
    }
}