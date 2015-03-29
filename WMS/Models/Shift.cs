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
    
    public partial class Shift
    {
        public Shift()
        {
            this.Emps = new HashSet<Emp>();
            this.RosterApps = new HashSet<RosterApp>();
        }
    
        public byte ShiftID { get; set; }
        public string ShiftName { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public byte DayOff1 { get; set; }
        public byte DayOff2 { get; set; }
        public bool Holiday { get; set; }
        public byte RosterType { get; set; }
        public short MonMin { get; set; }
        public short TueMin { get; set; }
        public short WedMin { get; set; }
        public short ThuMin { get; set; }
        public short FriMin { get; set; }
        public short SatMin { get; set; }
        public short SunMin { get; set; }
        public Nullable<short> LateIn { get; set; }
        public Nullable<short> EarlyIn { get; set; }
        public Nullable<short> EarlyOut { get; set; }
        public Nullable<short> LateOut { get; set; }
        public Nullable<short> OverTimeMin { get; set; }
        public Nullable<short> MinHrs { get; set; }
        public Nullable<bool> HasBreak { get; set; }
        public Nullable<short> BreakMin { get; set; }
        public Nullable<bool> GZDays { get; set; }
        public Nullable<bool> OpenShift { get; set; }
        public Nullable<short> CompanyID { get; set; }
    
        public virtual DaysName DaysName { get; set; }
        public virtual DaysName DaysName1 { get; set; }
        public virtual ICollection<Emp> Emps { get; set; }
        public virtual ICollection<RosterApp> RosterApps { get; set; }
        public virtual RosterType RosterType1 { get; set; }
    }
}
