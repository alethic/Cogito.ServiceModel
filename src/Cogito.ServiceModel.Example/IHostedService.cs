using System.ServiceModel;

namespace Cogito.ServiceModel.Example
{

    [ServiceContract]
    public interface IHostedService
    {

        [OperationContract]
        bool DoThing(string value);

    }

}