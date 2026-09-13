using System;
using System.ServiceModel;

using Microsoft.Extensions.DependencyInjection;

namespace Cogito.ServiceModel.DependencyInjection
{

    /// <summary>
    /// Adds dependency injection related methods to service hosts.
    /// </summary>
    public static class ServiceHostExtensions
    {

        /// <summary>
        /// Adds the custom service behavior required for dependency injection.
        /// </summary>
        /// <typeparam name="TService">The service contract type.</typeparam>
        /// <param name="serviceHost">The service host.</param>
        /// <param name="provider">The service provider.</param>
        public static void AddDependencyInjectionBehavior<TService>(this ServiceHostBase serviceHost, IServiceProvider provider)
        {
            AddDependencyInjectionBehavior(serviceHost, typeof(TService), provider);
        }

        /// <summary>
        /// Adds the custom service behavior required for dependency injection.
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <param name="serviceType">The service contract type.</param>
        /// <param name="provider">The service provider.</param>
        public static void AddDependencyInjectionBehavior(this ServiceHostBase serviceHost, Type serviceType, IServiceProvider provider)
        {
            if (serviceHost == null)
                throw new ArgumentNullException(nameof(serviceHost));
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            var serviceBehavior = serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            if (serviceBehavior != null && serviceBehavior.InstanceContextMode == InstanceContextMode.Single)
                return;

            var data = new ServiceImplementationData()
            {
                ConstructorString = serviceType.AssemblyQualifiedName,
                ServiceTypeToHost = serviceType,
                ImplementationResolver = l => l.GetRequiredService(serviceType)
            };

            var behavior = new DependencyInjectionServiceBehavior(provider, data);
            serviceHost.Description.Behaviors.Add(behavior);
        }

    }

}
