using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MackPortfolio.Extensions;

namespace MackPortfolio.Controllers
{
    public class ActivityController : BaseController
    {
        // GET: Activity
        public ActionResult Index()
        {
            var list = db.Events
                .Include(s => s.Location)
                .OrderByDescending(d => d.EventDate)
                .ToList();
            return View(list);
        }

        public ActionResult _activityList()
        {
            var list = db.Events
                .Include(s => s.Location)
                .OrderByDescending(d => d.EventDate)
                .ToList();
            return PartialView(list);
        }

        public JsonResult _activityListJson()
        {
            var list = db.Events
                .Include(s => s.Location)
                .OrderByDescending(d => d.EventDate)
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: /Activity/Details/5
        public ActionResult Details(string page, Guid? id, string urlparam)
        {
            if (id == null && page == "Details")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var article = db.Events
               .Include(d => d.Location)
               .SingleOrDefault(s => s.Id == id);

            if (article == null)
            {
                if (page == "Details")
                {
                    return HttpNotFound();
                }
                article = new Activity();
                article.Location = new Location();
            }
            ViewBag.page = page;
            return View(page, article);
        }

        // POST: /Activity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HostBy,HostEmail,HostPhone,EventDate,ImageUrl,Title,Summary,BodyHtml,UrlParameter,Created,Modified,IsActive")] Activity activity,
            [Bind(Include = "Address,Lat,Lng,LogMessages")]Location @location, FormCollection fc)
        {
            @location.GeoLocation = DbGeography.FromText(String.Format("POINT({0} {1})", @location.Lng.ToString(), @location.Lat.ToString()), 4326);
            activity.Location = @location;

            var file = Request.Files.AllKeys.Any() ? Request.Files[0] : null;
            string imgName;
            if (file != null && file.ContentLength > 0)
            {
                var image = new WebImage(file.InputStream);
                image.FileName = file.FileName;
                var serverPath = Server.MapPath("~/Media/Activity");
                imgName = image.CreateThumbnail(null, serverPath, "Activity");
            }
            else
            {
                imgName = "~/Images/no_Photo.png";
            }
            activity.ImageUrl = imgName;
            if (ModelState.IsValid)
            {
                var url = activity.EventDate.ToShortDateString() + "_" + activity.Title;
                activity.UrlParameter = url.ToUrlString("-");
                db.Events.Add(activity);
                db.SaveChanges();
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
        public ActionResult Edit([Bind(Include = "Id,HostBy,HostEmail,HostPhone,EventDate,ImageUrl,Title,Summary,BodyHtml,UrlParameter,Created,Modified,IsActive")] Activity activity,
            [Bind(Include = "LocationId,Address,Lat,Lng,LogMessages")]Location @location, FormCollection fc)
        {
            @location.GeoLocation = DbGeography.FromText(String.Format("POINT({0} {1})", @location.Lng.ToString(), @location.Lat.ToString()), 4326);
            activity.Location = @location;

            var file = Request.Files.AllKeys.Any() ? Request.Files[0] : null;
            var oldImg = activity.ImageUrl;
            string imgName;
            if (file != null && file.ContentLength > 0)
            {
                var image = new WebImage(file.InputStream);
                image.FileName = file.FileName;
                var serverPath = Server.MapPath("~/Media/Activity");
                imgName = image.CreateThumbnail(Server.MapPath(oldImg), serverPath, "Activity");
                activity.ImageUrl = imgName;
            }

            var url = activity.EventDate.ToShortDateString() + "_" + activity.Title;
            activity.UrlParameter = url.ToUrlString("-");

            if (ModelState.IsValid)
            {
                db.Entry(activity.Location).State = EntityState.Modified;
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.page = "Modify";
            return View("Modify", activity);
        }

        // GET: /Activity/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Events.Find(id);
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
            Activity activity = db.Events.Find(id);
            db.Events.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("_activityList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}