using System;
using System.Collections.Generic;
using System.ServiceModel;
using Autofac.Core;
using Autofac.Core.Registration;

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
    public class AutofacInstanceContext : IExtension<InstanceContext>, IDisposable, IComponentContext
    {
        private bool _disposed;

        /// <summary>
        /// Gets the current <see cref="Cogito.ServiceModel.DependencyInjection.AutofacInstanceContext"/>
        /// for the operation.
        /// </summary>
        /// <value>
        /// The <see cref="Cogito.ServiceModel.DependencyInjection.AutofacInstanceContext"/> associated
        /// with the current <see cref="System.ServiceModel.OperationContext"/> if
        /// one exists; or <see langword="null" /> if there isn't one.
        /// </value>
        /// <remarks>
        /// <para>
        /// In a singleton service, there won't be a current <see cref="Cogito.ServiceModel.DependencyInjection.AutofacInstanceContext"/>
        /// because singleton services are resolved at the time the service host begins
        /// rather than on each operation.
        /// </para>
        /// </remarks>
        public static AutofacInstanceContext Current
        {
            get
            {
                var operationContext = OperationContext.Current;
                if (operationContext != null)
                {
                    var instanceContext = operationContext.InstanceContext;
                    if (instanceContext != null)
                    {
                        return instanceContext.Extensions.Find<AutofacInstanceContext>();
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the request/operation lifetime.
        /// </summary>
        /// <value>
        /// An <see cref="Autofac.ILifetimeScope"/> that this instance
        /// context will use to resolve service instances.
        /// </value>
        public ILifetimeScope OperationLifetime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacInstanceContext"/> class.
        /// </summary>
        /// <param name="container">
        /// The outer container/lifetime scope from which the instance scope
        /// will be created.
        /// </param>
        public AutofacInstanceContext(ILifetimeScope container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.OperationLifetime = container.BeginLifetimeScope();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AutofacInstanceContext"/> class.
        /// </summary>
        ~AutofacInstanceContext()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Enables an extension object to find out when it has been aggregated.
        /// Called when the extension is added to the
        /// <see cref="System.ServiceModel.IExtensibleObject{T}.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Attach(InstanceContext owner)
        {
        }

        /// <summary>
        /// Enables an object to find out when it is no longer aggregated.
        /// Called when an extension is removed from the
        /// <see cref="System.ServiceModel.IExtensibleObject{T}.Extensions"/> property.
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
        /// of <see cref="Cogito.ServiceModel.DependencyInjection.AutofacInstanceContext.Dispose()"/>); or
        /// <see langword="false" /> if this is getting run as part of finalization where
        /// managed resources may have already been cleaned up.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // Free managed resources
                    this.OperationLifetime.Dispose();
                }
                this._disposed = true;
            }
        }

        /// <summary>
        /// Associates services with the components that provide them.
        /// </summary>
        public IComponentRegistry ComponentRegistry
        {
            get { return this.OperationLifetime.ComponentRegistry; }
        }

        /// <summary>
        /// Resolve an instance of the provided registration within the context.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <param name="parameters">Parameters for the instance.</param>
        /// <returns>
        /// The component instance.
        /// </returns>
        /// <exception cref="ComponentNotRegisteredException"/>
        /// <exception cref="Autofac.Core.DependencyResolutionException"/>
        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            return this.OperationLifetime.ResolveComponent(registration, parameters);
        }

        /// <summary>
        /// Retrieve a service instance from the context.
        /// </summary>
        /// <param name="serviceData">
        /// Data object containing information about how to resolve the service
        /// implementation instance.
        /// </param>
        /// <returns>The service instance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="serviceData" /> is <see langword="null" />.
        /// </exception>
        public object Resolve(ServiceImplementationData serviceData)
        {
            if (serviceData == null)
            {
                throw new ArgumentNullException("serviceData");
            }
            return serviceData.ImplementationResolver(this.OperationLifetime);
        }
    }
}
