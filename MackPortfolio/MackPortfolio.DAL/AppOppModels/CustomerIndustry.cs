namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.CustomerIndustry")]
    public partial class CustomerIndustry
    {
        public CustomerIndustry()
        {
            Customers = new HashSet<Customer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short VerticalCategoryID { get; set; }

        [Required]
        [StringLength(40)]
        public string VerticalCategoryName { get; set; }

        public byte? VerticalID { get; set; }

        [StringLength(40)]
        public string VerticalName { get; set; }

        public byte? IndustryID { get; set; }

        [StringLength(40)]
        public string IndustryName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
