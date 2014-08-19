namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.SalesDate")]
    public partial class SalesDate
    {
        public SalesDate()
        {
            Opportunities = new HashSet<Opportunity>();
            Opportunities1 = new HashSet<Opportunity>();
            ApplicationOpportunities = new HashSet<ApplicationOpportunity>();
            ApplicationOpportunities1 = new HashSet<ApplicationOpportunity>();
            UserApplications = new HashSet<UserApplication>();
            UserApplications1 = new HashSet<UserApplication>();
        }

        [Key]
        [Column(TypeName = "date")]
        public DateTime CalendarDate { get; set; }

        public DateTime CalendarDateTime { get; set; }

        [Required]
        [StringLength(20)]
        public string CalendarDateName { get; set; }

        public short FiscalMonthID { get; set; }

        [Required]
        [StringLength(18)]
        public string FiscalMonthName { get; set; }

        public byte FiscalQuarterID { get; set; }

        [Required]
        [StringLength(7)]
        public string FiscalQuarterName { get; set; }

        public byte FiscalYearID { get; set; }

        [Required]
        [StringLength(4)]
        public string FiscalYearName { get; set; }

        public virtual ICollection<Opportunity> Opportunities { get; set; }

        public virtual ICollection<Opportunity> Opportunities1 { get; set; }

        public virtual ICollection<ApplicationOpportunity> ApplicationOpportunities { get; set; }

        public virtual ICollection<ApplicationOpportunity> ApplicationOpportunities1 { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }

        public virtual ICollection<UserApplication> UserApplications1 { get; set; }
    }
}
