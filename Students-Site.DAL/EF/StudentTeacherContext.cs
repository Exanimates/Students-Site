using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.EF
{
    public class StudentTeacherContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public StudentTeacherContext(DbContextOptions<StudentTeacherContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentTeacher>()
                .HasKey(t => new { t.StudentId, t.TeacherId });

            modelBuilder.Entity<StudentTeacher>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentTeachers)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentTeacher>()
                .HasOne(sc => sc.Teacher)
                .WithMany(c => c.StudentTeachers)
                .HasForeignKey(sc => sc.StudentId);
        }
    }
}
