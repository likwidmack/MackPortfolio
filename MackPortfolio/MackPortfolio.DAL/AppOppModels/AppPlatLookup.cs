namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.AppPlatLookup")]
    public partial class AppPlatLookup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AppPlatLookupID { get; set; }

        public int? AppLookupID { get; set; }

        [Column("Application Name")]
        [StringLength(510)]
        public string Application_Name { get; set; }

        [Column("Publisher Name")]
        [StringLength(200)]
        public string Publisher_Name { get; set; }

        public int? PlatformID { get; set; }

        public int? LookupPublisherID { get; set; }

        [Column("Application Description")]
        public string Application_Description { get; set; }

        [Column("Store App ID")]
        [StringLength(132)]
        public string Store_App_ID { get; set; }

        [Column("WebSite URL")]
        [StringLength(137)]
        public string WebSite_URL { get; set; }

        [Column("DPE Pipe Status")]
        [StringLength(50)]
        public string DPE_Pipe_Status { get; set; }

        [Column("Application Source Key")]
        [StringLength(36)]
        public string Application_Source_Key { get; set; }

        [StringLength(17)]
        public string AppDataSource { get; set; }

        [Column("Days In Stage")]
        public int? Days_In_Stage { get; set; }

        [Column("Application Owner")]
        [StringLength(50)]
        public string Application_Owner { get; set; }

        [Column("Application Owner Email")]
        [StringLength(100)]
        public string Application_Owner_Email { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public int? AppStatusID { get; set; }

        public virtual AppLookup AppLookup { get; set; }

        public virtual AppStatus AppStatus { get; set; }

        public virtual Platform Platform { get; set; }

        public virtual PublishersLookup PublishersLookup { get; set; }
    }
}
