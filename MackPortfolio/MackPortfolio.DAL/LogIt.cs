using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MackPortfolio.DAL
{
    public abstract class LogIt
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Editable(false)]
        public string UrlParameter { get; set; }
        [Editable(false)]
        public DateTime Created { get; set; }
        [Editable(false)]
        public DateTime Modified { get; set; }
        [Display(Name = "Is Active", Description = "Is this item currently active?")]
        public bool IsActive { get; set; }
    }
}
