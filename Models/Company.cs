using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class Company
    {
        public Company()
        {
            Areas = new HashSet<Area>();
            Personnel = new HashSet<Personnel>();
        }

        [DisplayName("Company ID")]
        public int CompanyId { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Personnel> Personnel { get; set; }
    }
}
