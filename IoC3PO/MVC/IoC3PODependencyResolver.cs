using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace IoC3PO.MVC
{
    public class IoC3PODependencyResolver : IDependencyResolver
    {
        private readonly Container _container;

        public IoC3PODependencyResolver(Container container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }

    public class IoC3POControllerFactory : DefaultControllerFactory
    {
        private readonly Container _container;

        public IoC3POControllerFactory(Container container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return null;
            return (IController) _container.Resolve(controllerType);
        }
    }


}
