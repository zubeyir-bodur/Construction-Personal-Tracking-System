using System;
using System.Collections.Generic;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class PersonnelType
    {
        public PersonnelType()
        {
            Personnel = new HashSet<Personnel>();
        }

        public int PersonnelTypeId { get; set; }
        public string PersonnelTypeName { get; set; }

        public virtual ICollection<Personnel> Personnel { get; set; }
    }
}
