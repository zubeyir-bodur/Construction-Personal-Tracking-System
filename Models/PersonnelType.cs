using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Construction_Personal_Tracking_System.Models;
using System.ComponentModel.DataAnnotations.Schema;

// Change the path later for organization
namespace Construction_Personal_Tracking_System.Models
{
    public class PersonnelType
    {
        // Primary Key
        public int personnel_type_id { get; set; }
        public string personnel_type_name { get; set; }

        // Navigation Properties
        public ICollection<Personnel> personnels{ get; set; }
        

        public PersonnelType() { }
    }
}