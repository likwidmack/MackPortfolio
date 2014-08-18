using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;

namespace MackPortfolio
{
    public static class ODataConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // New code:
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            //builder.EntitySet<Product>("Products");
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}