using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class Tracking
    {
        [DisplayName("Tracking ID")]
        public int TrackingId { get; set; }
        [DisplayName("Personnel ID ")]
        public int PersonnelId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [DisplayName("Role")]
        public string PersonnelType { get; set; }
        [DisplayName("Company")]
        public string CompanyName { get; set; }
        [DisplayName("Area")]
        public string AreaName { get; set; }

        [DisplayName("Entrance Date")]
        public DateTime EntranceDate { get; set; }

        [DisplayName("Exit Date")]
        public DateTime? ExitDate { get; set; }

        [DisplayName("Auto Exit")]
        public bool AutoExit { get; set; }

        public virtual Personnel Personnel { get; set; }
    }
}
