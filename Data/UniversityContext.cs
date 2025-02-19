using Microsoft.EntityFrameworkCore;
using InmindLab3_4part2.Models;
using System.Collections.Generic;

namespace InmindLab3_4part2.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many config: Class <-> Student
            modelBuilder.Entity<Class>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassStudent",
                    j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId"),
                    j => j.HasOne<Class>().WithMany().HasForeignKey("ClassId")
                );
        }
    }
}