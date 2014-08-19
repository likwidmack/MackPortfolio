namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.CustomerSubSegment")]
    public partial class CustomerSubSegment
    {
        public CustomerSubSegment()
        {
            Customers = new HashSet<Customer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MarketSubSegmentId { get; set; }

        [StringLength(50)]
        public string SubSegmentName { get; set; }

        public int SegmentID { get; set; }

        [StringLength(30)]
        public string SegmentName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public virtual CustomerSegment CustomerSegment { get; set; }
    }
}
