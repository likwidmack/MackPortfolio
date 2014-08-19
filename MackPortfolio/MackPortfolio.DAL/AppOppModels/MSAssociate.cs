namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.MSAssociate")]
    public partial class MSAssociate
    {
        public MSAssociate()
        {
            OpportunityTeams = new HashSet<OpportunityTeam>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MSAssociateID { get; set; }

        [StringLength(100)]
        public string EmailAddress { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MicrosoftAlias { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime ETLCreatedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ETLModifiedDate { get; set; }

        public virtual ICollection<OpportunityTeam> OpportunityTeams { get; set; }
    }
}
