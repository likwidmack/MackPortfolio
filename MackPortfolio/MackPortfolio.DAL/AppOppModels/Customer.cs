namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.Customers")]
    public partial class Customer
    {
        public Customer()
        {
            Opportunities = new HashSet<Opportunity>();
        }

        public int GSXCustomerID { get; set; }

        [Key]
        [StringLength(15)]
        public string CRMCustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string CustomerOwnerAlias { get; set; }

        public bool? IsGlobalAccount { get; set; }

        public short? VerticalCategoryID { get; set; }

        public int? SubsidiaryDistrictId { get; set; }

        public int CustomerSubSegmentID { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual CustomerIndustry CustomerIndustry { get; set; }

        public virtual CustomerSubSegment CustomerSubSegment { get; set; }

        public virtual GeoHierarchy GeoHierarchy { get; set; }

        public virtual ICollection<Opportunity> Opportunities { get; set; }
    }
}
