using PumpService.Service;
using System.ServiceModel;

namespace PumpService.Interface
{
    [ServiceContract]
    public interface IPumpServiceCallback
    {
        [OperationContract]
        void UpdateStatistics(StatisticsService statistics);
    }
}
