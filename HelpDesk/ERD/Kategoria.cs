//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Helpdesk.ERD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Kategoria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategoria()
        {
            this.Zgloszenie = new HashSet<Zgloszenie>();
        }
    
        public int IdKategorii { get; set; }
        public string NazwaKategorii { get; set; }
        public string OpisKategorii { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zgloszenie> Zgloszenie { get; set; }
    }
}
