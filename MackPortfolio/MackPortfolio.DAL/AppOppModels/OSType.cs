namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.OSType")]
    public partial class OSType
    {
        public OSType()
        {
            UserApplications = new HashSet<UserApplication>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OSTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string OSTypeName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
