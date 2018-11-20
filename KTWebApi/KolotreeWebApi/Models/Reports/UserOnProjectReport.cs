using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models.Reports
{
    public class UserOnProjectReport
    {

        public User User { get; set; }
        public Project Project { get; set; }
        public int AssignedHours { get; set; }
        public int SpentHours { get; set; }
        public int AvailableHours { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
