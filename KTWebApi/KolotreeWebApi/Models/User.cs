using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
      
        public string FullName { get; set; }

       
    }
}
