using Autofac;
using Autofac.Integration.Mvc;
using Bogosoft.Mvc.Xsl.WebTest.Infrastructure;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Disposed(object sender, EventArgs args)
        {
            foreach(var x in Services.Disposables)
            {
                x.Dispose();
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));

            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(Services.ViewEngines);

            Disposed += Application_Disposed;
        }
    }
}