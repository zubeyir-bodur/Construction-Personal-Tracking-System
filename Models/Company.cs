using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Construction_Personal_Tracking_System.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

// Change the path later for organization
namespace Construction_Personal_Tracking_System.Models
{
    public class Company
    {
        // primary key
        public int company_id { get; set; }

        // attributes
        [Required]
        public string company_name { get; set; }

        // Navigation Properties
        public ICollection<Personnel> personnels { get; set; }
        public ICollection<Area> areas { get; set; }

        public Company() { }
    }
}