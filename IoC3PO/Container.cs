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
            _registeredTypes.Add(typeof(TInterface), new TypeRegistration(lifecycle, resolvedLifecycle, typeof(TImplementation)));
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
            return typeRegistration.createInstance(ctorArgs);
        }

        private object[] resolveCtorArguments(ConstructorInfo ctorInfo)
        {
            return ctorInfo
                .GetParameters()
                .Select(param => resolve(param.ParameterType))
                .ToArray();
        }

        private ILifeCycle resolveLifecycle(LifeCycle lifecycle)
        {
            // strategy pattern this?
            if (lifecycle == LifeCycle.Singleton)
            {
                return new SingletonLifecycle();
            }

            return null;
        }
    }

    public interface ILifeCycle
    {
        object CreateInstance(Type instanceType, object[] arguments);
    }

    public abstract class LifecycleBase
    {
        // rename as this causes confusion with the ILifeCycle create instance?
        protected object createInstance(Type instanceType, object[] arguments)
        {
            return Activator.CreateInstance(instanceType, arguments);
        }
    }

    public class SingletonLifecycle : LifecycleBase, ILifeCycle
    {
        private object singletonInstance;

        public object CreateInstance(Type instanceType, object[] arguments)
        {
            if (singletonInstance == null)
            {
                singletonInstance = createInstance(instanceType, arguments);
            }

            return singletonInstance;
        }
    }

    public class TypeRegistration
    {
        public LifeCycle LifeCycle { get; }
        public ILifeCycle NewLifeCycle { get; }
        public Type RegisteredType { get; }
        public object RegisteredObject { get; set; }

        public TypeRegistration(LifeCycle lifeCycle, ILifeCycle newLifeCycle, Type registeredType)
        {
            NewLifeCycle = newLifeCycle;
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
            if (LifeCycle == LifeCycle.Singleton)
            {
                return NewLifeCycle.CreateInstance(RegisteredType, arguments);
            }

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
