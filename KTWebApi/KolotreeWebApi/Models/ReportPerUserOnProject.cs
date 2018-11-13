using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class ReportPerUserOnProject
    {

        public User User { get; set; }
        public Project Project { get; set; }
        public int AssignedHours { get; set; }
        public int SpentHours { get; set; }
        public int TotalHours { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
