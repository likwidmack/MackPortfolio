using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MackPortfolio.DAL.Interfaces.Models
{
    class ActivityViewModel
    {
        public Guid Id { get; set; }
        [Editable(false)]
        public string UrlParameter { get; set; }
        [Editable(false)]
        public DateTime Created { get; set; }
        [Editable(false)]
        public DateTime Modified { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Host Name", Prompt = "Person's or Organization's Name", Description = "Enter the Host company, association or person of the Event")]
        public string HostBy { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address", Prompt = "email@address.com", Description = "Enter the Host email address")]
        public string HostEmail { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number", Prompt = "XXX-XXX-XXXX", Description = "Enter the Host phone number")]
        public string HostPhone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Event Start Date", Prompt = "MM/dd/yyyy", Description = "Enter the Start Date of the Event")]
        public DateTime EventDate { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Image or Logo", Description = "Select an Image or Logo; i.e., *.jpg, *.jpeg, *.bmp, *.png, *.gif")]
        public string ImageUrl { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Title", Prompt = "Title of Article", Description = "Enter Title of the Article")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Summary", Prompt = "A short summary of the article.*", Description = "Enter a Short Description of the Article")]
        public string Summary { get; set; }

        [DataType("EditorFull")]
        [Required, MaxLength, AllowHtml]
        [Display(Name = "Description", Prompt = "Body Description of the Article", Description = "Enter the Complete Information of the Article")]
        public string BodyHtml { get; set; }

        public int LocationId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Event Location", Prompt = "Street Address \nCity, State Zipcode", Description = "Enter Complete Address")]
        public string Address { get; set; }
        [DataType("Hidden")]
        public float Lat { get; set; }
        [DataType("Hidden")]
        public float Lng { get; set; }
        [DataType("Hidden")]
        public string LogMessages { get; set; }
        public DbGeography GeoLocation { get; set; }

        //public virtual ICollection<Media> Medias { get; set; }
    }
}
