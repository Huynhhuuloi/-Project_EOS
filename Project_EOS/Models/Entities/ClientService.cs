//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_EOS.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientService
    {
        public int ClientServiceID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<System.DateTime> ServiceStartDate { get; set; }
        public Nullable<System.DateTime> ServiceEndDate { get; set; }
        public Nullable<int> EmployeeID { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Service Service { get; set; }
    }
}
