namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.Platform")]
    public partial class Platform
    {
        public Platform()
        {
            AppPlatLookups = new HashSet<AppPlatLookup>();
        }

        public int PlatformID { get; set; }

        [Required]
        [StringLength(50)]
        public string PlatformName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<AppPlatLookup> AppPlatLookups { get; set; }
    }
}
