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
    
    public partial class Higi_Client_Mappings
    {
        public int MapId { get; set; }
        public int ClientId { get; set; }
        public int HigiUserId { get; set; }
        public int ClientUserId { get; set; }
        public System.DateTime DateTimeCreated { get; set; }
    
        public virtual Target Target { get; set; }
    }
}
