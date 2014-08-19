namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.CustomerSegment")]
    public partial class CustomerSegment
    {
        public CustomerSegment()
        {
            CustomerSubSegments = new HashSet<CustomerSubSegment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SegmentID { get; set; }

        [StringLength(100)]
        public string SegmentName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<CustomerSubSegment> CustomerSubSegments { get; set; }
    }
}
