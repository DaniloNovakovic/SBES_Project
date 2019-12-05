using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IPrimaryService
    {
        [OperationContract]
        void SendAlarm(Alarm alarm);
    }
}
