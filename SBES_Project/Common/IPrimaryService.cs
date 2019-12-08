using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IPrimaryService
    {
        [OperationContract]
        void SendAlarm(Alarm alarm);

        [OperationContract]
        List<Alarm> GetAlarms();

        [OperationContract]
        void RemoveAllAlarms();
    }
}
