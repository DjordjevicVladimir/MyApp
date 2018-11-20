using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace KolotreeWebApi.Models
{
    public class HoursRecordForCreation
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]       
        public int UserId { get; set; }
        [Required]
        [Range(0, 1000)]
        public int Hours { get; set; }
    }
}
