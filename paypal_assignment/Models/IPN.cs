//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace paypal_assignment.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class IPN
    {
        public string transactionID { get; set; }
        public Nullable<System.DateTime> txTime { get; set; }
        public string custom { get; set; }
        public string buyerEmail { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string paymentStatus { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public Nullable<int> quantity { get; set; }
    }
}
