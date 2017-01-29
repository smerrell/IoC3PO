using Xunit;
using IoC3P0Core;
using Shouldly;

namespace IoC3POCore.Tests
{
    public class ContainerTests
    {
        public ContainerTests()
        {
        }

        [Fact]
        public void does_it_work()
        {
            var container = new Container();
            container.Test().ShouldBeFalse();
        }
    }
}
