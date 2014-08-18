using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MackPortfolio.Controllers
{
    public class PaletteController : BaseController
    {
        // GET: Palette
        public ActionResult Index()
        {
            return View();
        }
    }
}