using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class HoursRecord
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public int AssignedHours { get; set; }
        public int SpentHours { get; set; }
        public DateTime Date { get; private set; }      

       public HoursRecord(int userId, int projecId)
       {
            ProjectId = projecId;
            UserId = userId;   
            Date = DateTime.Now;
       }

    public HoursRecord(int userId, int projecId, int assignedHours, int spentHours)
        {
            ProjectId = projecId;
            UserId = userId;
            AssignedHours = assignedHours;
            SpentHours = spentHours;
            Date = DateTime.Now;
        }

    }
}
