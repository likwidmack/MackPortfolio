namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.Opportunities")]
    public partial class Opportunity
    {
        public Opportunity()
        {
            ApplicationOpportunities = new HashSet<ApplicationOpportunity>();
            OpportunityDevices = new HashSet<OpportunityDevice>();
            OpportunityTeams = new HashSet<OpportunityTeam>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OpportunityID { get; set; }

        [Required]
        [StringLength(15)]
        public string CRMOpportunityID { get; set; }

        [Required]
        [StringLength(100)]
        public string OpportunityName { get; set; }

        public int? CustomerID { get; set; }

        [Required]
        [StringLength(15)]
        public string CRMCustomerID { get; set; }

        [StringLength(255)]
        public string OpportunityDescription { get; set; }

        [StringLength(50)]
        public string OpportunityOwnerAlias { get; set; }

        public int? SalesStageId { get; set; }

        [StringLength(50)]
        public string RepAssignedAlias { get; set; }

        public DateTime? OpportunityDueDateTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OpportunityDueDate { get; set; }

        public DateTime? ActualOpportunityCloseDateTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ActualOpportunityCloseDate { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<ApplicationOpportunity> ApplicationOpportunities { get; set; }

        public virtual SalesDate SalesDate { get; set; }

        public virtual SalesDate SalesDate1 { get; set; }

        public virtual SalesStage SalesStage { get; set; }

        public virtual ICollection<OpportunityDevice> OpportunityDevices { get; set; }

        public virtual ICollection<OpportunityTeam> OpportunityTeams { get; set; }
    }
}
