using System;
using System.Collections.Generic;
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
            var resolvedLifecycle = resolveLifecycle(lifecycle);
            _registeredTypes.Add(typeof(TInterface), new TypeRegistration(resolvedLifecycle, typeof(TImplementation)));
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface) Resolve(typeof(TInterface));
        }

        public object Resolve(Type contract)
        {
            if (!_registeredTypes.ContainsKey(contract))
            {
                throw new TypeNotRegisteredException($"The type {contract} is not registered.");
            }

            var typeRegistration = _registeredTypes[contract];
            var ctorArgs = resolveCtorArguments(typeRegistration.ResolveLongestConstructor());
            return typeRegistration.createInstance(ctorArgs);
        }

        private object[] resolveCtorArguments(ConstructorInfo ctorInfo)
        {
            return ctorInfo
                .GetParameters()
                .Select(param => Resolve(param.ParameterType))
                .ToArray();
        }

        private ILifeCycle resolveLifecycle(LifeCycle lifecycle)
        {
            // strategy pattern this?
            if (lifecycle == LifeCycle.Singleton)
            {
                return new SingletonLifecycle();
            }

            return new TransientLifecycle();
        }
    }

    public class TypeRegistration
    {
        public ILifeCycle Lifecycle { get; }
        public Type RegisteredType { get; }

        public TypeRegistration(ILifeCycle lifecycle, Type registeredType)
        {
            Lifecycle = lifecycle;
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
            return Lifecycle.CreateInstance(RegisteredType, arguments);
        }
    }

    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message) : base(message)
        {
        }
    }
}
