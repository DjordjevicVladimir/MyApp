using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models.Reports
{
    public class SumUsersReport
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int AssignedHoursSum { get; set; }
        public int SpentHoursSum { get; set; }
        public int AvailableHoursSum { get; set; }
        public List<UserHelperReport> Users { get; set; }       

    }
}
