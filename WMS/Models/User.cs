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
    
    public partial class User
    {
        public User()
        {
            this.LvApplications = new HashSet<LvApplication>();
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<byte> RoleID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> CanEdit { get; set; }
        public Nullable<bool> CanDelete { get; set; }
        public Nullable<bool> CanView { get; set; }
        public Nullable<short> CompanyID { get; set; }
        public Nullable<bool> CanAdd { get; set; }
        public Nullable<int> EmpID { get; set; }
        public Nullable<bool> MHR { get; set; }
        public Nullable<bool> MDevice { get; set; }
        public Nullable<bool> MLeave { get; set; }
        public Nullable<bool> MDesktop { get; set; }
        public Nullable<bool> MEditAtt { get; set; }
        public Nullable<bool> MUser { get; set; }
        public Nullable<bool> MOption { get; set; }
        public Nullable<bool> MRDailyAtt { get; set; }
        public Nullable<bool> MRLeave { get; set; }
        public Nullable<bool> MRMonthly { get; set; }
        public Nullable<bool> MRAudit { get; set; }
        public Nullable<bool> MRManualEditAtt { get; set; }
        public Nullable<bool> MREmployee { get; set; }
        public Nullable<bool> MRDetail { get; set; }
        public Nullable<bool> MRSummary { get; set; }
        public Nullable<bool> MRGraph { get; set; }
        public Nullable<bool> ViewPermanentStaff { get; set; }
        public Nullable<bool> ViewPermanentMgm { get; set; }
        public Nullable<bool> ViewContractual { get; set; }
        public Nullable<bool> ViewLocation { get; set; }
        public Nullable<short> LocationID { get; set; }
        public Nullable<bool> MRoster { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Emp Emp { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<LvApplication> LvApplications { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
