//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emergency
    {
        public int EmgID { get; set; }
        public string EmgNo { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> CloseDateTime { get; set; }
        public Nullable<int> UserID { get; set; }
        public string ComputerName { get; set; }
        public string IP { get; set; }
        public string EmgName { get; set; }
        public string EmgDesc { get; set; }
        public Nullable<int> TotalPresent { get; set; }
        public Nullable<int> TotalMissing { get; set; }
        public Nullable<int> TotalSaved { get; set; }
    }
}
