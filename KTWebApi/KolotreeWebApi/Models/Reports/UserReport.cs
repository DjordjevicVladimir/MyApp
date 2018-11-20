
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models.Reports
{
    public class UserReport
    {
        public User User { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }       
        public List<ProjectsHelperReport> UserWorkOnProjects { get; set; }
        public int AssignedHoursSum { get; set; }
        public int SpentHoursSum { get; set; }
        public int AvailableHoursSum { get; set; }      

    }
}
