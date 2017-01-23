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

        public interface IAstromechDroid { }

        [Fact]
        public void throws_informative_message_when_type_not_registered()
        {
            Should.Throw<TypeNotRegisteredException>(() =>
            {
                _container.Resolve<IAstromechDroid>();
            });
        }

        public interface IDroidVault { }
        public class DroidVault : IDroidVault { }
        public interface ICockpit { }
        public class Cockpit : ICockpit { }
        public interface ISandCrawler { }

        public class SandCrawler : ISandCrawler
        {
            public ICockpit Cockpit { get; }
            public IDroidVault DroidVault { get; }

            public SandCrawler(ICockpit cockpit, IDroidVault droidVault)
            {
                Cockpit = cockpit;
                DroidVault = droidVault;
            }
        }

        [Fact]
        public void resolves_constructor_arguments_when_registered()
        {
            _container.Register<ICockpit, Cockpit>();
            _container.Register<IDroidVault, DroidVault>();
            _container.Register<ISandCrawler, SandCrawler>();

            var crawler = _container.Resolve<ISandCrawler>() as SandCrawler;
            crawler.DroidVault.ShouldNotBeNull();
            crawler.Cockpit.ShouldNotBeNull();
        }
    }
}
