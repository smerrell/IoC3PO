using System;
using System.Linq;
using System.Web.Mvc;

namespace IoC3PO.MVC
{
    public static class TypeScanner
    {
        public static void ScanControllersInAssembly(this Container container, Type assemblyType)
        {
            var controllers = assemblyType
                .Assembly
                .ExportedTypes
                .Where(t => t.IsSubclassOf(typeof(Controller)))
                .ToList();

            foreach (var controller in controllers)
            {
                container.register(controller);
            }
        }
    }
}