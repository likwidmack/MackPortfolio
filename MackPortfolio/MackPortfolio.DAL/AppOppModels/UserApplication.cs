namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APP.UserApplications")]
    public partial class UserApplication
    {
        public UserApplication()
        {
            ApplicationOpportunities = new HashSet<ApplicationOpportunity>();
        }

        [Key]
        public int ApplicationID { get; set; }

        public int? AppLookupID { get; set; }

        public int? PublisherID { get; set; }

        [Column("Application Name")]
        [StringLength(500)]
        public string Application_Name { get; set; }

        [Column("Application Description")]
        [StringLength(2000)]
        public string Application_Description { get; set; }

        public int? AppStatusID { get; set; }

        [StringLength(100)]
        public string ApplicationWebsite { get; set; }

        public string NewRequestJustification { get; set; }

        [StringLength(100)]
        public string SideLoadURL { get; set; }

        public bool? SideLoadAvailable { get; set; }

        [StringLength(100)]
        public string AppVersion { get; set; }

        public int? OSTypeID { get; set; }

        public int? OSVersionID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ModifiedDate { get; set; }

        public int NativeOrWeb { get; set; }

        public virtual AppLookup AppLookup { get; set; }

        public virtual AppStatus AppStatus { get; set; }

        public virtual NativeOrWeb NativeOrWeb1 { get; set; }

        public virtual OSType OSType { get; set; }

        public virtual OSVersion OSVersion { get; set; }

        public virtual SalesDate SalesDate { get; set; }

        public virtual SalesDate SalesDate1 { get; set; }

        public virtual ICollection<ApplicationOpportunity> ApplicationOpportunities { get; set; }

        public virtual UserPublisher UserPublisher { get; set; }
    }
}
