using System;

namespace IoC3P0Core
{
    public enum LifeCycle
    {
        Transient,
        Singleton
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

        public SingletonLifecycle(object instance = null)
        {
            singletonInstance = instance;
        }

        public object CreateInstance(Type instanceType, object[] arguments)
        {
            if (singletonInstance == null)
            {
                singletonInstance = createInstance(instanceType, arguments);
            }

            return singletonInstance;
        }
    }

    public class TransientLifecycle : LifecycleBase, ILifeCycle
    {
        public object CreateInstance(Type instanceType, object[] arguments)
        {
            return createInstance(instanceType, arguments);
        }
    }
}