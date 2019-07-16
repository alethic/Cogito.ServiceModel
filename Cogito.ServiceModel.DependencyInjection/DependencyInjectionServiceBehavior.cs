using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Cogito.ServiceModel.DependencyInjection
{

    /// <summary>
    /// Sets the instance provider to an <see cref="DependencyInjectionInstanceProvider"/>.
    /// </summary>
    public class DependencyInjectionServiceBehavior : IServiceBehavior
    {

        readonly IServiceProvider provider;
        readonly ServiceImplementationData serviceData;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionServiceBehavior"/> class.
        /// </summary>
        /// <param name="provider">
        /// The container from which service implementations should be resolved.
        /// </param>
        /// <param name="serviceData">
        /// Data about which service type should be hosted and how to resolve the type to use for the service implementation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="provider" /> or <paramref name="serviceData" /> is <see langword="null" />.
        /// </exception>
        public DependencyInjectionServiceBehavior(IServiceProvider provider, ServiceImplementationData serviceData)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.serviceData = serviceData ?? throw new ArgumentNullException(nameof(serviceData));
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service
        /// can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param>
        /// <param name="serviceHostBase">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {

        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="serviceDescription" /> or
        /// <paramref name="serviceHostBase" /> is <see langword="null" />.
        /// </exception>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (serviceDescription == null)
                throw new ArgumentNullException(nameof(serviceDescription));
            if (serviceHostBase == null)
                throw new ArgumentNullException(nameof(serviceHostBase));

            var implementedContracts = serviceDescription.Endpoints
                 .Where(ep => ep.Contract.ContractType.IsAssignableFrom(serviceData.ServiceTypeToHost))
                 .Select(ep => ep.Contract.Name)
                 .ToArray();

            var instanceProvider = new DependencyInjectionInstanceProvider(provider, serviceData);

            var endpointDispatchers = serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>()
                .SelectMany(i => i.Endpoints)
                .Where(i => implementedContracts.Contains(i.ContractName));

            foreach (var ed in endpointDispatchers)
                ed.DispatchRuntime.InstanceProvider = instanceProvider;
        }

    }

}