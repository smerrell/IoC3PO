using Shouldly;
using Xunit;

namespace IoC3PO.Tests
{
    public class ContainerTests
    {
        public interface IProtocolDroid { }
        public class Threepio : IProtocolDroid { }

        [Fact]
        public void registers_implementation()
        {
            var container = new Container();
            container.Register<IProtocolDroid, Threepio>();

            var threepio = container.Resolve<IProtocolDroid>();
            threepio.ShouldNotBeNull();
            threepio.ShouldBeOfType<Threepio>();
        }
    }
}
