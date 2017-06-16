using Autofac;
using Autofac.Integration.Mvc;
using Bogosoft.Mvc.Xsl.WebTest.Infrastructure;
using System;
using System.Configuration;
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

            ViewEngines.Engines.Clear();

            ITransformProvider provider = new FileTransformProvider();

            if(ConfigurationManager.AppSettings["CacheXslTransforms"]?.ToLower() == "true")
            {
                var watch = ConfigurationManager.AppSettings["WatchForChangesInXslts"]?.ToLower() == "true";

                provider = new MemoryCachedTransformProvider(provider, watch);

                builder.RegisterInstance(provider).As<ICachedTransformProvider>();
            }

            var engine = XsltViewEngine.Create(Services.ViewLocations, provider.GetTransform)
                                       .Using(Services.XmlFormatter)
                                       .With(Services.DefaultViewParameters);

            ViewEngines.Engines.Add(engine);

            Disposed += Application_Disposed;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}