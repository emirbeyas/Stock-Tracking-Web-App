//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace REEDERSTAJ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KATEGORILER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KATEGORILER()
        {
            this.URUNLER = new HashSet<URUNLER>();
        }
    
        public int ID { get; set; }
        public string ACIKLAMA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<URUNLER> URUNLER { get; set; }
    }
}
