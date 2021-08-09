using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class Area
    {
        [DisplayName("Area ID")]
        public int AreaId { get; set; }
        [DisplayName("Area Name")]
        public string AreaName { get; set; }
        [DisplayName("Company ID")]
        public int CompanyId { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [DisplayName("QR Code")]
        public byte[] QrCode { get; set; }

        public virtual Company Company { get; set; }
    }
}
