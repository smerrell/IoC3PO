using System;
using System.Collections.Generic;
using System.Linq;

namespace IoC3PO
{
    public class Container
    {
        private Dictionary<Type, TypeRegistration> types = new Dictionary<Type, TypeRegistration>();
        public void Register<TInterface, TImplementation>()
        {
            Register<TInterface, TImplementation>(LifeCycle.Transient);
        }

        public void Register<TInterface, TImplementation>(LifeCycle lifecycle)
        {
            types.Add(typeof(TInterface), new TypeRegistration(lifecycle, typeof(TImplementation)));
        }

        public TInterface Resolve<TInterface>()
        {
            var interfaceType = typeof(TInterface);
            TypeRegistration typeRegistration;

            types.TryGetValue(interfaceType, out typeRegistration);
            if (typeRegistration == null) throw new TypeNotRegisteredException($"The interface {interfaceType} is not registered.");

            if (typeRegistration.LifeCycle == LifeCycle.Singleton)
            {
                if (typeRegistration.RegisteredObject == null)
                {
                    typeRegistration.RegisteredObject = Activator.CreateInstance(typeRegistration.RegisteredType);
                }

                return (TInterface) typeRegistration.RegisteredObject;
            }

            return (TInterface) Activator.CreateInstance(typeRegistration.RegisteredType);
        }
    }

    public class TypeRegistration
    {
        public LifeCycle LifeCycle { get; }
        public Type RegisteredType { get; }
        public object RegisteredObject { get; set; }

        public TypeRegistration(LifeCycle lifeCycle, Type registeredType)
        {
            LifeCycle = lifeCycle;
            RegisteredType = registeredType;
        }
    }

    public enum LifeCycle
    {
        Transient,
        Singleton
    }

    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message) : base(message)
        {
        }
    }
}
