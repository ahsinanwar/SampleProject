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
    
    public partial class Division
    {
        public Division()
        {
            this.Departments = new HashSet<Department>();
        }
    
        public short DivisionID { get; set; }
        public string DivisionName { get; set; }
        public Nullable<short> CompID { get; set; }
    
        public virtual ICollection<Department> Departments { get; set; }
    }
}
