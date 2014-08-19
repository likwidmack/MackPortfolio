using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MackPortfolio.DAL.ActivityModels
{
    [Table("ACT.EventLocation")]
    public class Location
    {
        [Key]
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
    }
}
