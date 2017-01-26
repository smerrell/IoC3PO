using System.Web.Mvc;
using IoC3PO.MVC;
using Shouldly;
using Xunit;

namespace IoC3PO.Tests
{
    public class TestController : Controller { }
    public class AnotherTestController : Controller { }
    public class NotAController { }

    public class MvcTests 
    {
        private Container _container;

        public MvcTests()
        {
            _container = new Container();
        }

        [Fact]
        public void scans_selected_assembly_for_types_inheriting_controller()
        {
            _container.ScanControllersInAssembly(typeof(TestController));
            _container.Resolve<TestController>().ShouldNotBeNull();
            _container.Resolve<AnotherTestController>().ShouldNotBeNull();
            Should.Throw<TypeNotRegisteredException>(() =>
            {
                _container.Resolve<NotAController>();
            });
        }
    }
}