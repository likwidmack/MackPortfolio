namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APP.UserPublishers")]
    public partial class UserPublisher
    {
        public UserPublisher()
        {
            UserApplications = new HashSet<UserApplication>();
        }

        [Key]
        public int PublisherID { get; set; }

        public int? PublisherLookupID { get; set; }

        [StringLength(200)]
        public string PublisherName { get; set; }

        [StringLength(100)]
        public string PublisherURL { get; set; }

        public virtual PublishersLookup PublishersLookup { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
