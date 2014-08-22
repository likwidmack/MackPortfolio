using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MackPortfolio.Extensions;
using MackPortfolio.DAL.Interfaces;
using MackPortfolio.DAL.Repositories;
using MackPortfolio.DAL.Interfaces.Models;

namespace MackPortfolio.Controllers
{
    public class ActivityController : BaseController
    {
        private IActivityRepository rep;
        // GET: Activity
        public ActionResult Index()
        {
            //rep = new ActivityRepository();
            //var list = rep.GetList();
            return View();
        }

        public ActionResult _activityList()
        {
            rep = new ActivityRepository();
            var list = rep.GetList();
            return PartialView(list);
        }

        public JsonResult _activityListJson()
        {
            rep = new ActivityRepository();
            var list = rep.GetList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: /Activity/Details/5
        public ActionResult Details(string page, Guid? id, string urlparam)
        {
            rep = new ActivityRepository();
            if (id == null && page == "Details")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var article = rep.GetArticle(id);
            if (article == null)
            {
                if (page == "Details")
                {
                    return HttpNotFound();
                }
                article = new ActivityViewModel();
            }
            ViewBag.page = page;
            return View(page, article);
        }

        // POST: /Activity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HostBy,HostEmail,HostPhone,EventDate,ImageUrl,Title,Summary,BodyHtml,UrlParameter,Created,Modified,IsActive,Address,Lat,Lng,LogMessages")] 
            ActivityViewModel activity, FormCollection fc)
        {
            rep = new ActivityRepository();
            var file = Request.Files.AllKeys.Any() ? Request.Files[0] : null;
            if (ModelState.IsValid)
            {
                activity = rep.CreateArticle(activity, file, Server);
                return RedirectToAction("Index");
            }

            ViewBag.page = "Modify";
            return View("Modify", activity);
        }

        // POST: /Activity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HostBy,HostEmail,HostPhone,EventDate,ImageUrl,Title,Summary,BodyHtml,UrlParameter,Created,Modified,IsActive,Address,Lat,Lng,LogMessages")] 
            ActivityViewModel activity, FormCollection fc)
        {
            rep = new ActivityRepository();
            var file = Request.Files.AllKeys.Any() ? Request.Files[0] : null;

            if (ModelState.IsValid)
            {
                activity = rep.EditArticle(activity, file, Server);
                return RedirectToAction("Index");
            }

            ViewBag.page = "Modify";
            return View("Modify", activity);
        }

        // GET: /Activity/Delete/5
        public ActionResult Delete(Guid? id)
        {
            rep = new ActivityRepository();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var activity = rep.GetArticle(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: /Activity/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            rep = new ActivityRepository();
            rep.DeleteArticle(id);
            return RedirectToAction("_activityList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rep.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}