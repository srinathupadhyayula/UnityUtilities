using System;
using System.Collections.Concurrent;

namespace Utilities.Patterns
{
    /// <inheritdoc cref="IServiceLocator"/>
    /// <remarks>
    /// An implementation of the <see cref="IServiceLocator"/> pattern.
    /// </remarks>
    public sealed class ServiceLocator : IServiceLocator
    {
        private readonly ConcurrentDictionary<Type, object> m_services = new();

        /// <inheritdoc cref="IServiceLocator.Get" />
        public T Get<T>() => (T)Get(typeof(T));

        /// <inheritdoc cref="IServiceLocator.Get{T}" />
        public object Get(Type type)
        {
            if (!m_services.ContainsKey(type))
            {
                throw new InvalidOperationException($"Service was never registered for {type.FullName} type.");
            }

            return m_services[type];
        }

        /// <inheritdoc cref="IServiceLocator.IsRegistered{T}" />
        public bool IsRegistered<T>() => IsRegistered(typeof(T));

        /// <inheritdoc cref="IServiceLocator.IsRegistered" />
        public bool IsRegistered(Type type)
        {
            return m_services.ContainsKey(type);
        }

        /// <inheritdoc cref="IServiceLocator.Register{T}" />
        public void Register<T>(T service) => Register(typeof(T), service);

        /// <inheritdoc cref="IServiceLocator.Register" />
        public void Register(Type type, object service)
        {
            if (m_services.ContainsKey(type))
            {
                throw new InvalidOperationException($"Service is already registered for {type.FullName} type.");
            }

            m_services.TryAdd(type, service);
        }

        /// <inheritdoc cref="IServiceLocator.Unregister{T}" />
        public void Unregister<T>() => Unregister(typeof(T));

        /// <inheritdoc cref="IServiceLocator.Unregister" />
        public void Unregister(Type type)
        {
            if (!m_services.ContainsKey(type))
            {
                throw new InvalidOperationException($"Service was never registered for {type.FullName} type.");
            }

            m_services.TryRemove(type, out var _);
        }

        /// <inheritdoc cref="IServiceLocator.Clear" />
        public void Clear() => m_services.Clear();
    }
}
