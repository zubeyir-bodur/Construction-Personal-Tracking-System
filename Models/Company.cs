using System;
using System.Collections.Generic;

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

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Personnel> Personnel { get; set; }
    }
}
