using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class KolotreeDbContext : DbContext
    {

        public KolotreeDbContext(DbContextOptions<KolotreeDbContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<HoursRecord> HoursRecords { get; set; }

    }
}
