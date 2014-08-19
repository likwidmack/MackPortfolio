using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MackPortfolio.DAL.WebModels
{
    [Table("WEB.UserMessages")]
    public class UserMessage : LogIt
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Subject", Prompt = "Subject of Message", Description = "Enter the subject of this message")]
        public string title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Your Name", Prompt = "Full Name", Description = "Enter your full name")]
        public string name { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address", Prompt = "email@address.com", Description = "Enter your email address")]
        public string email { get; set; }

        [Required]
        [AllowHtml]
        [DataType("EditorBasic")]
        [Display(Name = "Message", Prompt = "Summarize your request", Description = "Enter a short message")]
        public string msg { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number", Prompt = "222-555-1234", Description = "Enter your phone number")]
        public string phone { get; set; }
    }
}
