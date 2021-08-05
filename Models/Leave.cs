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
    public class Leave
    {
        // primary key
        public int leave_id { get; set; }

        // attributes
        [Required]
        [Column(TypeName = "smalldatetime")]
        public DateTime leave_start { get; set; }
        [Required]
        [Column(TypeName = "smalldatetime")]
        public DateTime leave_end { get; set; }

        // foreign key
        public int personnel_id { get; set; }

        // Navigation property
        public Personnel personnel { get; set; }
        public Leave() { }
    }
}