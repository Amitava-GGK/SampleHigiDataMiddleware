//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Higi.Middleware.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Target
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Target()
        {
            this.Higi_Client_Mappings = new HashSet<Higi_Client_Mappings>();
        }
    
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Endpoint { get; set; }
        public System.DateTime DateTimeCreated { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Higi_Client_Mappings> Higi_Client_Mappings { get; set; }
    }
}
