using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MackPortfolio.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/
        public ActionResult Index()
        {
            return View();
        }
        //HttpStatusCode 500
        public ViewResult Error()
        {
            return View();
        }

        //HttpStatusCode 404
        public ViewResult NotFound()
        {
            // http://stackoverflow.com/questions/619895/how-can-i-properly-handle-404-in-asp-net-mvc/2577095#2577095
            //// http://stackoverflow.com/questions/5995776/error-handling-in-asp-net-mvc-3

            // TODO: insert ILogger here

            return View();
        }

        //HttpStatusCode 401
        public ViewResult AccessDenied()
        {
            return View();
        }

        //HttpStatusCode 403
        public ViewResult Forbidden()
        {
            return View();
        }
    }
}