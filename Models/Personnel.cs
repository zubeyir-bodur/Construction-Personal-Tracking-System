using System;
using System.Collections.Generic;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class Personnel
    {
        public Personnel()
        {
            Leaves = new HashSet<Leave>();
            Trackings = new HashSet<Tracking>();
        }

        public int PersonnelId { get; set; }
        public string PersonnelName { get; set; }
        public string PersonnelSurname { get; set; }
        public int PersonnelTypeId { get; set; }
        public int CompanyId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual Company Company { get; set; }
        public virtual PersonnelType PersonnelType { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
    }
}
