using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KolotreeWebApi.Models
{
    [Table("Project")]
    public partial class Project
    {
        public Project()
        {
            HoursRecords = new HashSet<HoursRecord>();
        }

        public int ProjectId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [InverseProperty("Project")]
        public ICollection<HoursRecord> HoursRecords { get; set; }
    }
}
