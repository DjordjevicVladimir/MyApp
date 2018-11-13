using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class SumUsersReport
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalAssignedHours { get; set; }
        public int TotalSpentHours { get; set; }
        public int TotalHours { get; set; }
        public List<SimpleUserReport> users { get; set; }       

    }
}
