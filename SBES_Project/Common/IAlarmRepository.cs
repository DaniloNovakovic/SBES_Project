using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IAlarmRepository
    {
        IEnumerable<Alarm> GetAll();

        void Add(string serviceId, Alarm alarm);

        void DeleteAllByClientName(string clientName);

        void DeleteAll();
    }
}