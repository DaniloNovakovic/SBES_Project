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

        [OperationContract]
        void RemoveClientAlarms();

        [OperationContract]
        List<string> GetClientRemovalRequests();
        
        [OperationContract]
        void ApprovedRemoval(string clientName);

        [OperationContract]
        void DeniedRemoval(string clientName);
    }
}
