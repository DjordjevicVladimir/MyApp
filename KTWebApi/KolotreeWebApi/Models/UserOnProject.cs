using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class UserOnProject
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }      

       public  UserOnProject(int userId, int projecId, int hours)
        {
            ProjectId = projecId;
            UserId = userId;
            Hours = hours;              
            Date = DateTime.Now;
        }

    }
}
