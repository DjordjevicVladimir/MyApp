using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class UserForProject
    {
        
            public User User { get; set; }
            public int AssignedHours { get; set; }
            public int SpentHours { get; set; }
            public int TotalHours { get; set; }
        
    }
}
