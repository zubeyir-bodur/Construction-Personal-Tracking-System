using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class PersonnelType
    {
        public PersonnelType()
        {
            Personnel = new HashSet<Personnel>();
        }

        [DisplayName("Personnel Type ID")]
        public int PersonnelTypeId { get; set; }
        [DisplayName("Role")]
        public string PersonnelTypeName { get; set; }

        public virtual ICollection<Personnel> Personnel { get; set; }
    }
}
