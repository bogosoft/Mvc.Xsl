using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl.WebTest.Controllers
{
    public class CacheController : Controller
    {
        ICachedTransformProvider provider;

        public CacheController(ICachedTransformProvider provider)
        {
            this.provider = provider;
        }

        public ActionResult Clear()
        {
            provider.Clear();

            foreach(var x in GetCachedFilenames())
            {
                System.IO.File.Delete(x);
            }

            return RedirectToAction("Index", "Home");
        }

        protected IEnumerable<string> GetCachedFilenames()
        {
            return GetFlattenedFilenames(new DirectoryInfo(Server.MapPath("~/content/cached")));
        }

        protected IEnumerable<string> GetFlattenedFilenames(DirectoryInfo di)
        {
            foreach(var x in di.GetFiles())
            {
                yield return x.FullName;
            }

            foreach(var x in di.GetDirectories())
            {
                foreach(var y in GetFlattenedFilenames(x))
                {
                    yield return y;
                }
            }
        }
    }
}