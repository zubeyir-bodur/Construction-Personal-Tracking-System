using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        [DisplayName("Personnel ID")]
        public int PersonnelId { get; set; }

        [DisplayName("Name")]
        public string PersonnelName { get; set; }
        [DisplayName("Surname")]
        public string PersonnelSurname { get; set; }

        [DisplayName("Role ID")]
        public int PersonnelTypeId { get; set; }
        [DisplayName("Company ID")]
        public int CompanyId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual Company Company { get; set; }
        public virtual PersonnelType PersonnelType { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
    }
}
