namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("A4D.PublishersLookup")]
    public partial class PublishersLookup
    {
        public PublishersLookup()
        {
            AppPlatLookups = new HashSet<AppPlatLookup>();
            UserPublishers = new HashSet<UserPublisher>();
        }

        [Key]
        public int LookupPublisherID { get; set; }

        [StringLength(36)]
        public string MSPublisherKey { get; set; }

        [StringLength(200)]
        public string PublisherName { get; set; }

        [StringLength(100)]
        public string PublisherDataSource { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLModifiedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ETLCreatedDate { get; set; }

        public virtual ICollection<AppPlatLookup> AppPlatLookups { get; set; }

        public virtual ICollection<UserPublisher> UserPublishers { get; set; }
    }
}
