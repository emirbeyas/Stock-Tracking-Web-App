using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REEDERSTAJ.Models
{
    public class VMFaturalar
    {

        public FATURALAR GetFaturalars { get; set; }

        public IEnumerable<FIYATDEGISTIR> GetFIYATDEGISTIR { get; set; }

        public IEnumerable<SEPETSATIR> GetSiparis { get; set; }



    }
}