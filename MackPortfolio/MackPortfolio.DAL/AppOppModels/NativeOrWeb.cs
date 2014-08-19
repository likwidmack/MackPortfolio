namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.NativeOrWeb")]
    public partial class NativeOrWeb
    {
        public NativeOrWeb()
        {
            UserApplications = new HashSet<UserApplication>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NativeOrWebID { get; set; }

        [Required]
        [StringLength(100)]
        public string NativeOrWebName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
