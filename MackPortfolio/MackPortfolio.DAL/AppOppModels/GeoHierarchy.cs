namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.GeoHierarchy")]
    public partial class GeoHierarchy
    {
        public GeoHierarchy()
        {
            Customers = new HashSet<Customer>();
        }

        public int AreaId { get; set; }

        [Required]
        [StringLength(35)]
        public string AreaName { get; set; }

        public int SubsidiaryId { get; set; }

        [Required]
        [StringLength(40)]
        public string SubsidiaryName { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SubsidiaryDistrictId { get; set; }

        [Required]
        [StringLength(60)]
        public string SubsidiaryDistrictName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
