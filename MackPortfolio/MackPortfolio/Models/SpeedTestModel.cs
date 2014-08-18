using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MackPortfolio.Models
{
    public class SpeedTestModel
    {
        public SpeedTestModel(string im, string sc, long sz, byte[] f)
        {
            img = im;
            src = sc;
            size = sz;
            file = f;
        }
        public string img { get; set; }
        public string src { get; set; }
        public long size { get; set; }
        public byte[] file { get; set; }
    }
}