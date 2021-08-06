using System;
using System.Collections.Generic;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class Leave
    {
        public int LeaveId { get; set; }
        public DateTime LeaveStart { get; set; }
        public DateTime LeaveEnd { get; set; }
        public int PersonnelId { get; set; }

        public virtual Personnel Personnel { get; set; }
    }
}
