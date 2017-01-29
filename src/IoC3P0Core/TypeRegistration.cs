using System;
using System.Linq;
using System.Reflection;

namespace IoC3P0Core
{
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
                .GetTypeInfo()
                .GetConstructors()
                .OrderBy(x => x.GetParameters().Length)
                .First();
        }

        public object createInstance(object[] arguments)
        {
            return Lifecycle.CreateInstance(RegisteredType, arguments);
        }
    }
}