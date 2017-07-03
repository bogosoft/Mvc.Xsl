using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var infrastructure = ViewEnginesConfig.Configure().ToArray();

            DependencyResolver.SetResolver(ServicesConfig.GetDependencyResolver(infrastructure));
        }
    }
}