namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.DeviceType")]
    public partial class DeviceType
    {
        public DeviceType()
        {
            OpportunityDevices = new HashSet<OpportunityDevice>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeviceTypeID { get; set; }

        [StringLength(100)]
        public string DeviceTypeName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<OpportunityDevice> OpportunityDevices { get; set; }
    }
}
