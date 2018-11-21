using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KolotreeWebApi.Models
{
    [Table("HoursRecords")]
    public partial class HoursRecord
    {

        public int Id { get; set; }
        [Required]
        public Project Project { get; set; }
        [Required]
        public User User { get; set; }
        public int AssignedHours { get; set; }
        public int SpentHours { get; set; }
        [DataType(DataType.Date)]   
        public DateTime Date { get; private set; }


        public HoursRecord()
        {
            Date = DateTime.Now.Date;
        }

        public HoursRecord(User user, Project project)
        {
            this.User = user;
            this.Project = project;
        }
    }
}
