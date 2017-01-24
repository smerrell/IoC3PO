using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IoC3PO
{
    public class Container
    {
        private readonly Dictionary<Type, TypeRegistration> _registeredTypes = new Dictionary<Type, TypeRegistration>();

        public void Register<TContract, TImplementation>()
        {
            Register<TContract, TImplementation>(LifeCycle.Transient);
        }

        public void Register<TContract, TImplementation>(LifeCycle lifecycle)
        {
            assertContracts(typeof(TContract), typeof(TImplementation));
            var resolvedLifecycle = resolveLifecycle(lifecycle);
            _registeredTypes.Add(typeof(TContract), new TypeRegistration(resolvedLifecycle, typeof(TImplementation)));
        }

        private void assertContracts(Type contract, Type resolvedType)
        {
            if (!contract.IsAssignableFrom(resolvedType))
            {
                throw new TypeNotAssignableToContractException($"{resolvedType} is not assignable to {contract}. Unable to Register type.");
            }
        }

        public TContract Resolve<TContract>()
        {
            return (TContract) Resolve(typeof(TContract));
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
}
