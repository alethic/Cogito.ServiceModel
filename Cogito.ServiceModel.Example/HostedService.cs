using System;

namespace Cogito.ServiceModel.Example
{

    public class HostedService : IHostedService
    {
        private readonly IServiceProvider p;

        public HostedService(IServiceProvider p)
        {
            this.p = p ?? throw new ArgumentNullException(nameof(p));
        }

        public bool DoThing(string value)
        {
            return true;
        }

    }

}