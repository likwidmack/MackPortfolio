namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.SalesStage")]
    public partial class SalesStage
    {
        public SalesStage()
        {
            Opportunities = new HashSet<Opportunity>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SalesStageID { get; set; }

        [Required]
        [StringLength(57)]
        public string SalesStageName { get; set; }

        public int SalesStageFilterID { get; set; }

        public int IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ETLCreatedDate { get; set; }

        public virtual ICollection<Opportunity> Opportunities { get; set; }

        public virtual SalesStageFilter SalesStageFilter { get; set; }
    }
}
