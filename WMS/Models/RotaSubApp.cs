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
    
    public partial class RotaSubApp
    {
        public RotaSubApp()
        {
            this.Rosters = new HashSet<Roster>();
        }
    
        public int RotaSubAppID { get; set; }
        public Nullable<System.DateTime> DutyDate { get; set; }
        public Nullable<short> WorkMins { get; set; }
        public Nullable<System.TimeSpan> DutyTime { get; set; }
        public Nullable<int> RosterAppID { get; set; }
    
        public virtual ICollection<Roster> Rosters { get; set; }
        public virtual RosterApp RosterApp { get; set; }
    }
}
