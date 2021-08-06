using System;
using System.Collections.Generic;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class Tracking
    {
        public int TrackingId { get; set; }
        public int PersonnelId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PersonnelType { get; set; }
        public string CompanyName { get; set; }
        public string AreaName { get; set; }
        public DateTime EntranceDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public bool AutoExit { get; set; }

        public virtual Personnel Personnel { get; set; }
    }
}
