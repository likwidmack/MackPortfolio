namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.OpportunityTeam")]
    public partial class OpportunityTeam
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OpportunityId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MSAssociateId { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime ETLCreatedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ETLModifiedDate { get; set; }

        public virtual MSAssociate MSAssociate { get; set; }

        public virtual Opportunity Opportunity { get; set; }
    }
}
