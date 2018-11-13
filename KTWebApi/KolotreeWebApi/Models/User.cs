using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KolotreeWebApi.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            HoursRecords = new HashSet<HoursRecord>();
        }

        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [InverseProperty("User")]
        public ICollection<HoursRecord> HoursRecords { get; set; }
    }
}
