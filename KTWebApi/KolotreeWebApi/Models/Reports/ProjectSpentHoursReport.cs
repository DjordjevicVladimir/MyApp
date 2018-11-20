using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models.Reports
{
    public class ProjectSpentHoursReport
    {
        public Project Project { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<SpentHoursRecord> spentHoursRecords { get; set; }

        public class SpentHoursRecord
        {
            public User User { get; set; }
            public int SpentHours { get; set; }
            public DateTime DateOfRecord { get; set; }
        }

    }
}
