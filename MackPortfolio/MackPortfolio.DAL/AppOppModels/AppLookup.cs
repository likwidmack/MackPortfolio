namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.AppLookup")]
    public partial class AppLookup
    {
        public AppLookup()
        {
            AppPlatLookups = new HashSet<AppPlatLookup>();
            UserApplications = new HashSet<UserApplication>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AppLookupID { get; set; }

        [Column("Application Name")]
        [StringLength(510)]
        public string Application_Name { get; set; }

        [Column("Publisher Name")]
        [StringLength(200)]
        public string Publisher_Name { get; set; }

        public int? LookupPublisherID { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<AppPlatLookup> AppPlatLookups { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
