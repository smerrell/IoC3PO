using System;
using System.Collections.Generic;

namespace IoC3PO
{
    public class Container
    {
        private Dictionary<Type, object> types = new Dictionary<Type, object>();
        public void Register<TInterface, TImplementation>()
        {
            var type = Activator.CreateInstance<TImplementation>();
            types.Add(typeof(TInterface), type);
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface) types[typeof(TInterface)];
        }
    }

    public enum LifeCycle
    {
        Transient,
        Singleton
    }
}
