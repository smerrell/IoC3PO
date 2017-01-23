using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace IoC3PO
{
    public class Container
    {
        private readonly Dictionary<Type, TypeRegistration> _registeredTypes = new Dictionary<Type, TypeRegistration>();

        public void Register<TInterface, TImplementation>()
        {
            Register<TInterface, TImplementation>(LifeCycle.Transient);
        }

        public void Register<TInterface, TImplementation>(LifeCycle lifecycle)
        {
            // create a lifecycle object here?
            _registeredTypes.Add(typeof(TInterface), new TypeRegistration(lifecycle, typeof(TImplementation)));
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface) resolve(typeof(TInterface));
        }

        private object resolve(Type contract)
        {
            if (!_registeredTypes.ContainsKey(contract))
            {
                throw new TypeNotRegisteredException($"The type {contract} is not registered.");
            }

            var typeRegistration = _registeredTypes[contract];
            var ctorArgs = resolveCtorArguments(typeRegistration.ResolveLongestConstructor());

            // push this out of here and down into the TypeRegistration
            if (typeRegistration.LifeCycle == LifeCycle.Singleton)
            {
                if (typeRegistration.RegisteredObject == null)
                {
                    typeRegistration.RegisteredObject = typeRegistration.createInstance(ctorArgs);
                }

                return typeRegistration.RegisteredObject;
            }

            return typeRegistration.createInstance(ctorArgs);
        }

        private object[] resolveCtorArguments(ConstructorInfo ctorInfo)
        {
            return ctorInfo
                .GetParameters()
                .Select(param => resolve(param.ParameterType))
                .ToArray();
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

        public ConstructorInfo ResolveLongestConstructor()
        {
            return RegisteredType
                .GetConstructors()
                .OrderBy(x => x.GetParameters().Length)
                .First();
        }

        public object createInstance(object[] arguments)
        {
            // use lifecycle here?
            return Activator.CreateInstance(RegisteredType, arguments);
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
