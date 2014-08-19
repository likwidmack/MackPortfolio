namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.AppStatus")]
    public partial class AppStatus
    {
        public AppStatus()
        {
            AppPlatLookups = new HashSet<AppPlatLookup>();
            UserApplications = new HashSet<UserApplication>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StatusID { get; set; }

        [StringLength(100)]
        public string StatusName { get; set; }

        [StringLength(10)]
        public string AppStatusSeq { get; set; }

        public virtual ICollection<AppPlatLookup> AppPlatLookups { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
