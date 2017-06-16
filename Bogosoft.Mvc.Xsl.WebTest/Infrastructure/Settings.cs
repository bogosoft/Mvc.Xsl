using System.Linq;
using Config = System.Configuration.ConfigurationManager;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    static class Settings
    {
        static string[] Truths = new[] { "1", "on", "true", "yes" };

        internal static bool CacheLocalTransforms => IsTrue(Config.AppSettings["CacheLocalTransforms"]);

        internal static bool WatchForLocalChanges => IsTrue(Config.AppSettings["WatchForLocalChanges"]);

        static bool IsTrue(string value)
        {
            return Truths.Contains(value?.ToLower() ?? "");
        }
    }
}