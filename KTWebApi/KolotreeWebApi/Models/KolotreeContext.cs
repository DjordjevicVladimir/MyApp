using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KolotreeWebApi.Models
{
    public partial class KolotreeContext : DbContext
    {
        public KolotreeContext()
        {
        }

        public KolotreeContext(DbContextOptions<KolotreeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HoursRecord> HoursRecords { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HoursRecord>(entity =>
            {
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HoursRecords)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoursReco__Proje__3A81B327");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HoursRecords)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoursReco__UserI__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
