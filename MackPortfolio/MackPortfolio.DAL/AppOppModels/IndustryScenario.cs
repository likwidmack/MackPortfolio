namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.IndustryScenario")]
    public partial class IndustryScenario
    {
        public IndustryScenario()
        {
            ApplicationOpportunities = new HashSet<ApplicationOpportunity>();
        }

        [Key]
        public int ScenarioID { get; set; }

        [StringLength(100)]
        public string Sector { get; set; }

        [StringLength(100)]
        public string Industry { get; set; }

        [StringLength(100)]
        public string Scenario { get; set; }

        public virtual ICollection<ApplicationOpportunity> ApplicationOpportunities { get; set; }
    }
}
