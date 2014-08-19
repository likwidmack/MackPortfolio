using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MackPortfolio.DAL.WebModels
{
    [Table("WEB.WebContentRevisions")]
    public class WebContentRevision
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength]
        public string Json { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public int RevisionCounter { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
