using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        private object Resolve(Type contract)
        {
            if (!_registeredTypes.ContainsKey(contract))
            {
                throw new TypeNotRegisteredException($"The type {contract} is not registered.");
            }

            var typeRegistration = _registeredTypes[contract];
            if (typeRegistration.LifeCycle == LifeCycle.Singleton)
            {
                if (typeRegistration.RegisteredObject == null)
                {
                    typeRegistration.RegisteredObject = createInstance(typeRegistration.RegisteredType);
                }

                return typeRegistration.RegisteredObject;
            }

            return createInstance(typeRegistration.RegisteredType);
        }

        private object createInstance(Type typeToCreate)
        {
            var longestCtor = typeToCreate
                .GetConstructors()
                .OrderBy(x => x.GetParameters().Length)
                .First();

            var ctorArgs = longestCtor
                .GetParameters()
                .Select(param => Resolve(param.ParameterType))
                .ToArray();

            return Activator.CreateInstance(typeToCreate, ctorArgs);
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
