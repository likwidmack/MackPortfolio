using MackPortfolio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MackPortfolio.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult inetConnTest()
        {
            //string msg = "";
            string image = "milkywayband_gleason.jpg";
            var filepath = Path.Combine("~/Media/", image);
            var name = Path.GetFileName(filepath);

            filepath = Url.Content(filepath);
            FileInfo f = new FileInfo(Server.MapPath(filepath));
            FileStream file = new FileStream(f.FullName, FileMode.Open, FileAccess.Read);
            BinaryReader binaryFile = new BinaryReader(file);
            var filesize = (Int32)f.Length;
            var buffer = binaryFile.ReadBytes((Int32)filesize);
            var data = new SpeedTestModel(f.Name, f.DirectoryName, filesize, buffer)
            {
                img = f.Name,
                src = f.DirectoryName,
                size = filesize,
                file = buffer
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}