namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APP.ApplicationOpportunity")]
    public partial class ApplicationOpportunity
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApplicationID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OpportunityID { get; set; }

        public int? IndustryScenarioID { get; set; }

        public string PeripheralsBlockingDeal { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateAdded { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateUpdated { get; set; }

        public virtual IndustryScenario IndustryScenario { get; set; }

        public virtual Opportunity Opportunity { get; set; }

        public virtual SalesDate SalesDate { get; set; }

        public virtual SalesDate SalesDate1 { get; set; }

        public virtual UserApplication UserApplication { get; set; }
    }
}
