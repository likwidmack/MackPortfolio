using MackPortfolio.DAL;
using MackPortfolio.DAL.ActivityModels;
using MackPortfolio.DAL.Interfaces;
using MackPortfolio.DAL.Interfaces.Models;
using MackPortfolio.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;

namespace MackPortfolio.DAL.Repositories
{
    public class ActivityRepository : IActivityRepository, IDisposable
    {
        private MackPortfolioContext context = new MackPortfolioContext();

        public ActivityRepository() { }

        internal IQueryable<Activity> Activities
        {
            get { return context.Events.Include(s=>s.Location); }
        }
        public List<ActivityViewModel> GetList()
        {
            return Activities
                .Select(s => mapViewModel(s))
                .OrderByDescending(o => o.EventDate)
                .ToList();
        }
        public ActivityViewModel GetArticle(Guid? id)
        {
            var article = getArticle(id);
            return mapViewModel(article);
        }
        public ActivityViewModel CreateArticle(ActivityViewModel model, HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            var article = mapArticle(model);
            model.ImageUrl = saveFileImage(false, article.ImageUrl, file, server);
            var url = article.EventDate.ToShortDateString() + "_" + article.Title;
            article.UrlParameter = url.ToUrlString("-");

            context.Events.Add(article);
            context.SaveChanges();

            return mapViewModel(article);
        }
        public ActivityViewModel EditArticle(ActivityViewModel model, HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            var article = mapArticle(model);
            model.ImageUrl = saveFileImage(true, article.ImageUrl, file, server);
            var url = article.EventDate.ToShortDateString() + "_" + article.Title;
            article.UrlParameter = url.ToUrlString("-");

            context.Entry(article.Location).State = EntityState.Modified;
            context.Entry(article).State = EntityState.Modified;
            context.SaveChanges();

            return mapViewModel(article);
        }
        public void DeleteArticle(Guid? id)
        {
            var article = getArticle(id);
            context.Events.Remove(article);
            context.SaveChanges();
        }

        private Activity getArticle(Guid? id)
        {
            return Activities.SingleOrDefault(s => s.Id == id);
        }

        #region Mapping Models & Objects
        private ActivityViewModel mapViewModel(Activity data)
        {
            return new ActivityViewModel
            {
                Id = data.Id,
                UrlParameter = data.UrlParameter,
                Created = data.Created,
                Modified = data.Modified,
                IsActive = data.IsActive,
                EventDate = data.EventDate,
                Title = data.Title,
                Summary = data.Summary,
                HostBy = data.HostBy,
                HostEmail = data.HostEmail,
                HostPhone = data.HostPhone,
                ImageUrl = data.ImageUrl,
                BodyHtml = data.BodyHtml,
                LocationId = data.Location.LocationId,
                Address = data.Location.Address,
                Lat = data.Location.Lat,
                Lng = data.Location.Lng,
                GeoLocation = data.Location.GeoLocation.AsText(),
                LogMessages = data.Location.LogMessages
            };
        }
        private Activity mapArticle(ActivityViewModel model)
        {
            return new Activity
            {
                Id = model.Id,
                BodyHtml = model.BodyHtml,
                UrlParameter = model.UrlParameter,
                Created = model.Created,
                Modified = model.Modified,
                HostBy = model.HostBy,
                HostEmail = model.HostEmail,
                HostPhone = model.HostPhone,
                Summary = model.Summary,
                EventDate = model.EventDate,
                Title = model.Title,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive,
                Location = mapLocation(model)
            };
        }
        private Location mapLocation(ActivityViewModel model)
        {
            return new Location
            {
                LocationId = model.LocationId,
                Address = model.Address,
                Lat = model.Lat,
                Lng = model.Lng,
                LogMessages = model.LogMessages,
                GeoLocation = DbGeography.FromText(String.Format("POINT({0} {1})", model.Lng.ToString(), model.Lat.ToString()), 4326)
            };
        }
        #endregion

        private string saveFileImage(bool isEdit, string oldImgPath, HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            string imgName = null;
            if (file != null && file.ContentLength > 0)
            {
                var image = new WebImage(file.InputStream);
                image.FileName = file.FileName;
                var serverPath = server.MapPath("~/Media/Activity");
                var oldPath = isEdit ? server.MapPath(oldImgPath) : null;
                imgName = image.CreateThumbnail(oldImgPath, serverPath, "Activity");
            }
            else
            {
                if (!isEdit)
                {
                    imgName = "~/Images/no_Photo.png";
                }
            }
            return imgName;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
    }
}
