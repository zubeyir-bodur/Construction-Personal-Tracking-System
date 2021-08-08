using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class Leave
    {
        [DisplayName("Leave ID")]
        public int LeaveId { get; set; }

        [DisplayName("Leave Start Date")]
        public DateTime LeaveStart { get; set; }

        [DisplayName("Leave End Date")]
        public DateTime LeaveEnd { get; set; }

        [DisplayName("Personnel ID")]
        public int PersonnelId { get; set; }

        public virtual Personnel Personnel { get; set; }
    }
}
