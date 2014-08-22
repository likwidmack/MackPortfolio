using MackPortfolio.DAL.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MackPortfolio.DAL.Interfaces
{
    public interface IActivityRepository
    {
        List<ActivityViewModel> GetList();
        ActivityViewModel GetArticle(Guid? id);
        ActivityViewModel CreateArticle(ActivityViewModel model, HttpPostedFileBase file, HttpServerUtilityBase server);
        ActivityViewModel EditArticle(ActivityViewModel model, HttpPostedFileBase file, HttpServerUtilityBase server);
        void DeleteArticle(Guid? id);
        void Dispose();
    }
}
