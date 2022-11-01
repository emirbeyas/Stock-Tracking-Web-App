using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REEDERSTAJ.Models
{
    public class SepetVM
    {
        public IEnumerable<SEPETSATIR> GetSEPETSATIR { get; set; }

        public SEPET GetSEPET { get; set; }
    }
}