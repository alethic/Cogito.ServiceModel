using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Cogito.ServiceModel.DependencyInjection
{

    /// <summary>
    /// Retrieves service instances from an Autofac container.
    /// </summary>
    public class DependencyInjectionInstanceProvider : IInstanceProvider
    {

        readonly IServiceProvider provider;
        readonly ServiceImplementationData serviceData;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionInstanceProvider"/> class.
        /// </summary>
        /// <param name="provider">
        /// The lifetime scope from which service instances should be resolved.
        /// </param>
        /// <param name="serviceData">
        /// Data object containing information about how to resolve the service
        /// implementation instance.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="provider" /> or <paramref name="serviceData" /> is <see langword="null" />.
        /// </exception>
        public DependencyInjectionInstanceProvider(IServiceProvider provider, ServiceImplementationData serviceData)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (serviceData == null)
                throw new ArgumentNullException(nameof(serviceData));

            this.provider = provider;
            this.serviceData = serviceData;
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <returns>A user-defined service object.</returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="instanceContext" /> is <see langword="null" />.
        /// </exception>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            if (instanceContext == null)
                throw new ArgumentNullException(nameof(instanceContext));

            var autofacInstanceContext = new DependencyInjectionInstanceContext(provider);
            instanceContext.Extensions.Add(autofacInstanceContext);

            try
            {
                return autofacInstanceContext.Resolve(serviceData);
            }
            catch (Exception)
            {
                autofacInstanceContext.Dispose();
                instanceContext.Extensions.Remove(autofacInstanceContext);
                throw;
            }
        }

        /// <summary>
        /// Called when an <see cref="InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="instanceContext" /> is <see langword="null" />.
        /// </exception>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instanceContext == null)
                throw new ArgumentNullException(nameof(instanceContext));

            var extension = instanceContext.Extensions.Find<DependencyInjectionInstanceContext>();
            if (extension != null)
                extension.Dispose();
        }

    }

}