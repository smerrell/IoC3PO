using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IoC3PO.MVC;
using IoC3PO.Web.Controllers;

namespace IoC3PO.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new Container();
            container.Register<IDroid, Artoo>();
            container.Register<ISandCrawler, SandCrawler>();

            container.ScanControllersInAssembly(typeof(HomeController));

            //DependencyResolver.SetResolver(new IoC3PODependencyResolver(container));
            ControllerBuilder.Current.SetControllerFactory(new IoC3POControllerFactory(container));
        }
    }
}
