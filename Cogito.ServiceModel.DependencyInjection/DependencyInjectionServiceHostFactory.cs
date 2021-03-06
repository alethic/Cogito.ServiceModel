﻿using System;
using System.ServiceModel;

namespace Cogito.ServiceModel.DependencyInjection
{

    /// <summary>
    /// Creates <see cref="ServiceHost"/> instances for WCF.
    /// </summary>
    public class DependencyInjectionServiceHostFactory : DependencyInjectionHostFactory
    {

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
        /// </summary>
        /// <param name="serviceType">Specifies the type of service to host.</param>
        /// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="serviceType" /> or <paramref name="baseAddresses" /> is <see langword="null" />.
        /// </exception>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (baseAddresses == null)
                throw new ArgumentNullException(nameof(baseAddresses));

            return new ServiceHost(serviceType, baseAddresses);
        }

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
        /// </summary>
        /// <param name="singletonInstance">Specifies the singleton service instance to host.</param>
        /// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the singleton service instance specified with a specific base address.
        /// </returns>
        protected override ServiceHost CreateSingletonServiceHost(object singletonInstance, Uri[] baseAddresses)
        {
            if (singletonInstance == null)
                throw new ArgumentNullException(nameof(singletonInstance));
            if (baseAddresses == null)
                throw new ArgumentNullException(nameof(baseAddresses));

            return new ServiceHost(singletonInstance, baseAddresses);
        }

    }

}
