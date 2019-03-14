using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.EF
{
    public class UserContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public UserContext(DbContextOptions<StudentTeacherContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            builder.Entity<Teacher>()
                .HasIndex(u => u.UserId)
                .IsUnique();
        }
    }
}
