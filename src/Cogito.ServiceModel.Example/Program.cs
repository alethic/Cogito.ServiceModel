using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Cogito.ServiceModel.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Cogito.ServiceModel.Example
{

    public static class Program
    {

        public static void Main(string[] args)
        {
            var c = new ServiceCollection();
            c.AddTransient<HostedService>();
            var p = c.BuildServiceProvider();

            using (var h = new ServiceHost(typeof(HostedService), new Uri("http://localhost:12833")))
            {
                h.AddDependencyInjectionBehavior<HostedService>(p);

                var smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                h.Description.Behaviors.Add(smb);

                var sdb = h.Description.Behaviors.Find<ServiceDebugBehavior>();
                sdb.IncludeExceptionDetailInFaults = true;

                h.Open();

                Console.ReadLine();
            }
        }

    }

}
