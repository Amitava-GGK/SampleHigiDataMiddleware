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
    
    public partial class ClientRequestResponse
    {
        public int Mapping_ID { get; set; }
        public Nullable<int> MessageId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> DateTimeCreated { get; set; }
        public Nullable<int> ClientId { get; set; }
    
        public virtual QueueMessageStatu QueueMessageStatu { get; set; }
    }
}
