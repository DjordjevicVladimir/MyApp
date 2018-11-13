using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class ReportPerProject
    {
        
        public Project Project { get; set; }
        public List<SimpleUserReport> Users { get; set; }
        public int TotalAssignedHours { get; set; }
        public int TotalSpentHours { get; set; }
        public int TotalHours { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        
    }
}
