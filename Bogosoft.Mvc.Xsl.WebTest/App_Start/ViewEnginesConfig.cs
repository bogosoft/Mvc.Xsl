using System.Collections.Generic;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    static class ViewEnginesConfig
    {
        internal static IEnumerable<object> Configure()
        {
            ViewEngines.Engines.Clear();

            var engine = new DefaultViewEngineFactory().Build();

            ViewEngines.Engines.Add(engine);

            engine.ParameterizingView += ViewEngine_ParameterizingView;

            yield return engine.TransformProvider;
        }

        static void ViewEngine_ParameterizingView(ParameterizingViewEventArgs args)
        {
            args.Parameters["action"] = args.Context.RouteData.Values["action"];
            args.Parameters["controller"] = args.Context.RouteData.Values["controller"];
        }
    }
}