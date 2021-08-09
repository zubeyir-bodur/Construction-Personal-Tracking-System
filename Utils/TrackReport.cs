using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construction_Personal_Tracking_System.Utils
{
    public class TrackReport
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public string Area { get; set; }
        public string EntranceDate { get; set; }
        public string ExitDate { get; set; }

        public enum Exit
        {
            Automatic,
            Manual,
            NotExitedYet
        }

        public Exit ExitType { get; set; }
    }
}
