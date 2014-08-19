namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.SalesStageFilter")]
    public partial class SalesStageFilter
    {
        public int SalesStageFilterID { get; set; }

        [Required]
        [StringLength(100)]
        public string SalesStageFilterName { get; set; }

        public virtual SalesStage SalesStage { get; set; }
    }
}
