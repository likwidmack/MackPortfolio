using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MackPortfolio.Extensions
{
    public static class RequestBrowser
    {
        /// <summary>
        /// Get Html Class with Internet Explorer confirmation and versioning for use of CSS and UI calls
        /// </summary>
        /// <param name="request"></param>
        /// <returns>String</returns>
        public static string GetHtmlClass(this HttpRequestBase request)
        {
            return BrowserCompatibility.GetHtmlClass(request.Browser);
        }
        /// <summary>
        /// Determine if current user browser is Internet Explorer with conditional version compatibility
        /// </summary>
        /// <param name="request">Http Request</param>
        /// <param name="lessThanVersion">Optional: Determine if current IE version less than the given argument</param>
        /// <returns>Boolean</returns>
        public static bool IsIE(this HttpRequestBase request, int? lessThanVersion = null)
        {
            return BrowserCompatibility.IsIE(request.Browser, lessThanVersion);
        }

        public static bool CompareVersion(this HttpRequestBase request, Func<int, bool> compareVersion)
        {
            return compareVersion(request.Browser.MajorVersion);
        }
    }

    public class BrowserCompatibility
    {
        private string htmlclassproperty { get; set; }
        private const int LatestIEVersion = 11;
        private const int IEVersionFloor = 6;
        private static HttpBrowserCapabilitiesBase _browser;
        private string[] _ieArray = new string[] { "ie", "internetexplorer" };

        public BrowserCompatibility()
        {
        }

        public BrowserCompatibility(HttpRequestBase request)
        {
            _browser = request.Browser;
        }

        internal HttpRequestBase _request { get; set; }

        private static readonly Lazy<BrowserCompatibility> InstanceField = new Lazy<BrowserCompatibility>(() => new BrowserCompatibility());
        internal static BrowserCompatibility Instance
        {
            get
            {
                return InstanceField.Value;
            }
        }

        //Get Html Class string
        public static string GetHtmlClass()
        {
            return Instance.getHtmlClass();
        }
        public static string GetHtmlClass(HttpBrowserCapabilitiesBase browser)
        {
            if (browser != null) _browser = browser;
            return Instance.getHtmlClass();
        }
        private string getHtmlClass()
        {
            htmlclassproperty = "no-js";
            if (isIE(null))
            {
                htmlclassproperty += " ie";
                int i = LatestIEVersion;
                while (i-- > IEVersionFloor)
                {
                    if (lt(i))
                    {
                        htmlclassproperty += " lt-ie" + i;
                    }
                }
            }
            return htmlclassproperty;
        }

        //Determine if User Browser is IE
        public static bool IsIE(int? version = null)
        {
            return Instance.isIE(version);
        }
        public static bool IsIE(HttpBrowserCapabilitiesBase browser, int? version = null)
        {
            if (browser != null) _browser = browser;
            return Instance.isIE(version);
        }

        internal string Name
        {
            get
            {
                return _browser.Browser;
            }
        }

        private bool isIE(int? version)
        {
            var isIe = _ieArray.Contains(Name.ToLower());
            if (version.HasValue && version > 0)
            {
                return isIe && lt(version.Value);
            }
            return isIe;
        }

        //Determine IE browser verson
        internal int Version
        {
            get
            {
                return _browser.MajorVersion;
            }
        }

        private bool lt(int i)
        {
            return Version < i;
        }
    }
}
