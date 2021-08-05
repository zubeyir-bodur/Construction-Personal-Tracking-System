using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Construction_Personal_Tracking_System.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

// Change the path later for organization
namespace Construction_Personal_Tracking_System.Models
{
    public class Area
    {
        // primary key
        public int area_id { get; set; }

        // attributes
        [Required]
        public string area_name { get; set; }
        public double latitude { get; set; }
        public double longtitude { get; set; }
        // Necessary to install System.Drawing.Common from NuGet for Image class
        [Required]
        public Image qr_code { get; set; }

        // foreign key for the company
        public int company_id { get; set; }

        // Navigation Properties
        public Company company { get; set; }

        public Area() { }

    }
}