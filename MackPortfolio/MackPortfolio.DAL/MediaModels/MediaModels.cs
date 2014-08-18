using MackPortfolio.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MackPortfolio.MediaModels.DAL
{
    [Table("MMF.Media")]
    public class Media : LogIt
    {
        public string Title { get; set; }
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Extension { get; set; }
        [DataType("FileUrl")]
        public string Directory { get; set; }
        public string ContentType { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public bool IsPrimary { get; set; }

        public virtual Thumbnail Thumbnail { get; set; }
        public virtual Standard Standard { get; set; }
        public virtual Original Original { get; set; }
    }

    public abstract class Identity
    {
        [Key]
        public int id { get; set; }
        public string url { get; set; }
        public int size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    [Table("MMF.Thumbnails")]
    public class Thumbnail : Identity
    {
        //icon max 160px wide and 120px height
    }

    [Table("MMF.Standards")]
    public class Standard : Identity
    {
        //standard 800px wide and 600px height
    }

    [Table("MMF.Originals")]
    public class Original : Identity
    {
        //Original size
    }
}
