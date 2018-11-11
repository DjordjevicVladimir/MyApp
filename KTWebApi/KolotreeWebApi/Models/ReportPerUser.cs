using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class ReportPerUser
    {
        public User User { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<ProjectForUser> ProjectsForUser { get; set; }


        public class ProjectForUser
        {
            public Project Project { get; set; }
            public int AssignedHours { get; set; }
            public int SpentHours { get; set; }
        }
    }
}
