using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KolotreeWebApi.Models
{
    [Table("HoursRecord")]
    public partial class HoursRecord
    {

        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int UserId { get; set; }
        public int AssignedHours { get; set; }
        public int SpentHours { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; private set; }

        [ForeignKey("ProjectId")]
        [InverseProperty("HoursRecords")]
        public Project Project { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("HoursRecords")]
        public User User { get; set; }

        public HoursRecord(int userId, int projectId)
        {
            UserId = userId;
            ProjectId = projectId;
            Date = DateTime.Now.Date;
        }
    }
}
