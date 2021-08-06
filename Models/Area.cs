using System;
using System.Collections.Generic;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class Area
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public byte[] QrCode { get; set; }

        public virtual Company Company { get; set; }
    }
}
