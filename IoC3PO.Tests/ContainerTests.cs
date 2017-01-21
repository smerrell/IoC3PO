using Shouldly;
using Xunit;

namespace IoC3PO.Tests
{
    public class ContainerTests
    {
        private Container _container;

        public interface IProtocolDroid { }
        public class Threepio : IProtocolDroid { }

        public ContainerTests()
        {
            _container = new Container();
        }

        [Fact]
        public void registers_implementation()
        {
            _container.Register<IProtocolDroid, Threepio>();

            var threepio = _container.Resolve<IProtocolDroid>();
            threepio.ShouldNotBeNull();
            threepio.ShouldBeOfType<Threepio>();
        }

        [Fact]
        public void resolving_a_transient_object_returns_a_new_object_each_time()
        {
            _container.Register<IProtocolDroid, Threepio>();
            var threepio1 = _container.Resolve<IProtocolDroid>();
            var threepio2 = _container.Resolve<IProtocolDroid>();
            threepio1.ShouldNotBeSameAs(threepio2);
        }

        [Fact]
        public void registered_singleton_is_a_singleton()
        {
            _container.Register<IProtocolDroid, Threepio>(LifeCycle.Singleton);
            var threepio1 = _container.Resolve<IProtocolDroid>();
            var threepio2 = _container.Resolve<IProtocolDroid>();
            threepio1.ShouldBeSameAs(threepio2);
        }
    }
}
