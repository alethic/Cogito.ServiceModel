using System;
using System.Collections.Generic;
using System.ServiceModel;

using Microsoft.Extensions.DependencyInjection;

namespace Cogito.ServiceModel.DependencyInjection
{

    /// <summary>
    /// Manages instance lifecycle using an Autofac inner container.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This instance context extension creates a child lifetime scope based
    /// on a scope provided and resolves service instances from that child scope.
    /// </para>
    /// <para>
    /// When this instance context is disposed, the lifetime scope it creates
    /// (which contains the resolved service instance) is also disposed.
    /// </para>
    /// </remarks>
    public class DependencyInjectionInstanceContext : IExtension<InstanceContext>, IDisposable, IServiceProvider
    {

        bool _disposed;

        /// <summary>
        /// Gets the current <see cref="DependencyInjectionInstanceContext"/>
        /// for the operation.
        /// </summary>
        /// <value>
        /// The <see cref="DependencyInjectionInstanceContext"/> associated
        /// with the current <see cref="OperationContext"/> if
        /// one exists; or <see langword="null" /> if there isn't one.
        /// </value>
        /// <remarks>
        /// <para>
        /// In a singleton service, there won't be a current <see cref="DependencyInjectionInstanceContext"/>
        /// because singleton services are resolved at the time the service host begins
        /// rather than on each operation.
        /// </para>
        /// </remarks>
        public static DependencyInjectionInstanceContext Current
        {
            get
            {
                var operationContext = OperationContext.Current;
                if (operationContext != null)
                {
                    var instanceContext = operationContext.InstanceContext;
                    if (instanceContext != null)
                        return instanceContext.Extensions.Find<DependencyInjectionInstanceContext>();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the request/operation lifetime.
        /// </summary>
        /// <value>
        /// An <see cref="IServiceProvider"/> that this instance
        /// context will use to resolve service instances.
        /// </value>
        public IServiceScope OperationLifetime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionInstanceContext"/> class.
        /// </summary>
        /// <param name="provider">
        /// The outer container/lifetime scope from which the instance scope
        /// will be created.
        /// </param>
        public DependencyInjectionInstanceContext(IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            OperationLifetime = provider.CreateScope();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DependencyInjectionInstanceContext"/> class.
        /// </summary>
        ~DependencyInjectionInstanceContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Enables an extension object to find out when it has been aggregated.
        /// Called when the extension is added to the
        /// <see cref="IExtensibleObject{T}.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Attach(InstanceContext owner)
        {
        }

        /// <summary>
        /// Enables an object to find out when it is no longer aggregated.
        /// Called when an extension is removed from the
        /// <see cref="IExtensibleObject{T}.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Detach(InstanceContext owner)
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles disposal of managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true" /> to dispose of managed resources (during a manual execution
        /// of <see cref="Dispose()"/>); or
        /// <see langword="false" /> if this is getting run as part of finalization where
        /// managed resources may have already been cleaned up.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    OperationLifetime.Dispose();

                _disposed = true;
            }
        }

        /// <summary>
        /// Resolve an instance of the provided registration within the context.
        /// </summary>
        /// <param name="serviceType">Type of service to resolve.</param>
        /// <returns>
        /// The component instance.
        /// </returns>
        public object GetService(Type serviceType)
        {
            return OperationLifetime.ServiceProvider.GetService(serviceType);
        }

        /// <summary>
        /// Retrieve a service instance from the context.
        /// </summary>
        /// <param name="serviceData">
        /// Data object containing information about how to resolve the service
        /// implementation instance.
        /// </param>
        /// <returns>The service instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="serviceData" /> is <see langword="null" />.
        /// </exception>
        public object Resolve(ServiceImplementationData serviceData)
        {
            if (serviceData == null)
                throw new ArgumentNullException(nameof(serviceData));

            return serviceData.ImplementationResolver(OperationLifetime.ServiceProvider);
        }

    }

}
