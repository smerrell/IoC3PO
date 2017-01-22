using System;
using System.Collections.Generic;

namespace IoC3PO
{
    public class Container
    {
        private Dictionary<Type, TypeRegistration> _registeredTypes = new Dictionary<Type, TypeRegistration>();

        public void Register<TInterface, TImplementation>()
        {
            Register<TInterface, TImplementation>(LifeCycle.Transient);
        }

        public void Register<TInterface, TImplementation>(LifeCycle lifecycle)
        {
            _registeredTypes.Add(typeof(TInterface), new TypeRegistration(lifecycle, typeof(TImplementation)));
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface) Resolve(typeof(TInterface));
        }

        public object Resolve(Type typeToResolve)
        {
            if (!_registeredTypes.ContainsKey(typeToResolve))
            {
                throw new TypeNotRegisteredException($"The interface {typeToResolve} is not registered.");
            }

            var typeRegistration = _registeredTypes[typeToResolve];
            if (typeRegistration.LifeCycle == LifeCycle.Singleton)
            {
                if (typeRegistration.RegisteredObject == null)
                {
                    typeRegistration.RegisteredObject = Activator.CreateInstance(typeRegistration.RegisteredType);
                }

                return typeRegistration.RegisteredObject;
            }

            return Activator.CreateInstance(typeRegistration.RegisteredType);
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
