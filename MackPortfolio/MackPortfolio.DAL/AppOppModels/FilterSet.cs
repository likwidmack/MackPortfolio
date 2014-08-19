namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.FilterSet")]
    public partial class FilterSet
    {
        [Key]
        public int FilterID { get; set; }

        [StringLength(100)]
        public string FilterName { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [Column(TypeName = "ntext")]
        public string JsonString { get; set; }

        public bool? IsDefault { get; set; }
    }
}
