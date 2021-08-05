using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Construction_Personal_Tracking_System.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Construction_Personal_Tracking_System.Utils;
using System.ComponentModel.DataAnnotations;

// Change the path later for organization
namespace Construction_Personal_Tracking_System.Models
{
    public class Personnel
    {
        // primary key
        public int personnel_id { get; set; }

        // attributes

        [Required]
        public string personnel_name { get; set; }

        [Required]
        public string personnel_surname { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
        // Do we use UserInfo in the database or in model?
        //public UserInfo userInfo { get; set; }

        // foreign keys
        public int company_id { get; set; }
        public int personnel_type_id { get; set; }


        // Navigation Properties
        public Company company{ get; set; }
        public PersonnelType personnelType { get; set; }
        // Leave should allow nulls, so it is not required
        public Leave leave { get; set; }
        public ICollection<Tracking> trackings { get; set; }

        public Personnel() { }
    }
}