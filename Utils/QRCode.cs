using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
/// QRCode object information
/// </summary>
/// <author>Furkan Calik</author>
namespace Construction_Personal_Tracking_System.Utils {
    public class QRCode {
        public string Username { get; set; }
        public string CompanyName { get; set; }
        public string SectorName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitute { get; set; }
    }
}
