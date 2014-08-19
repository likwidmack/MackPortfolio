using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MackPortfolio.DAL.WebModels
{
    [Table("WEB.WebContents")]
    public class WebContent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string UrlParameter { get; set; }
        public string Area { get; set; }
        public string Folder { get; set; }
        public string Page { get; set; }
        public string Section { get; set; }
        public int Sequence { get; set; }
        public int ActiveVersion { get; set; }

        [MaxLength]
        public string Json { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<WebContentRevision> Revisions { get; set; }
    }
}
