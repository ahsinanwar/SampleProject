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
    
    public partial class EmergencyDetail
    {
        public int EmgDetailID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public Nullable<int> EmgID { get; set; }
        public Nullable<bool> SafeStatus { get; set; }
        public Nullable<short> SafeLocID { get; set; }
    }
}