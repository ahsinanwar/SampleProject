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
    
    public partial class City
    {
        public City()
        {
            this.Sites = new HashSet<Site>();
        }
    
        public short CityID { get; set; }
        public string CityName { get; set; }
        public Nullable<byte> RegionID { get; set; }
    
        public virtual Region Region { get; set; }
        public virtual ICollection<Site> Sites { get; set; }
    }
}
