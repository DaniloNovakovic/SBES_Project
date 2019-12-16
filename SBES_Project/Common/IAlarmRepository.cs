using System.Collections.Generic;

namespace Common
{
    public interface IAlarmRepository
    {
        IEnumerable<Alarm> GetAllByServiceId(string serviceId);

        void Add(string serviceId, Alarm alarm);

        void DeleteAllByClientName(string clientName);

        void DeleteAll();
    }
}