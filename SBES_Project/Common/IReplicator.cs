using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IReplicator
    {
        [OperationContract]
        void SendAlarm(Alarm alarm);

        [OperationContract]
        void CheckForConnection();
    }
}
