using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    static class ServicesConfig
    {
        internal static IDependencyResolver GetDependencyResolver(object[] infrastructure)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            foreach(var x in infrastructure)
            {
                builder.RegisterInstance(x).AsImplementedInterfaces();
            }

            return new AutofacDependencyResolver(builder.Build());
        }
    }
}