using System.Web.Optimization;

namespace MackPortfolio
{
    public static class Foundation
    {
        public static Bundle Scripts()
        {
            return new ScriptBundle("~/libs/foundation").Include(
                      "~/Scripts/foundation/fastclick.js",
                      "~/Scripts/jquery.cookie.js",
                      "~/Scripts/foundation/foundation.js",
                      "~/Scripts/foundation/foundation.*",
                      "~/Scripts/foundation/app.js");
        }
    }
}