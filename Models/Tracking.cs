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
    public class Tracking
    {
        // primary key
        public int tracking_id { get; set; }


        // attributes
        [Required]
        public string name { get; set; }
        [Required]
        public string surname { get; set; }
        [Required]
        public string personnel_type { get; set; }
        [Required]
        public string company_name { get; set; }
        [Required]
        public string area_name { get; set; }
        [Required]
        [Column(TypeName = "smalldatetime")]
        public DateTime entrance_date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime exit_date { get; set; }
        public bool auto_exit { get; set; }

        // foreign keys
        public int personnel_id { get; set; }

        // navigation properties
        public Personnel personnel { get; set; }
        
        public Tracking() { }
    }
}