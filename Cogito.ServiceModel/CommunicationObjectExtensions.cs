using System.ServiceModel;
using System.Threading.Tasks;

namespace Cogito.ServiceModel
{

    public static class CommunicationObjectExtensions
    {

        public static Task OpenAsync(this ICommunicationObject self)
        {
            return Task.Factory.FromAsync(self.BeginOpen, self.EndOpen, null);
        }

        public static Task CloseAsync(this ICommunicationObject self)
        {
            return Task.Factory.FromAsync(self.BeginClose, self.EndClose, null);
        }

    }

}
