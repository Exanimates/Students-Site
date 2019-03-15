using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.EF
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<StudentTeacher> StudentTeachers { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roles = new List<Role>
            {
                new Role { Name = "Dean" },
                new Role { Name = "Student" },
                new Role { Name = "Teacher" },
            };

            var users = new List<User>
            {
                new User { FirstName = "Petr", Password = "123", LastName = "Ivanov", Role = roles[0] },
                new User { FirstName = "Andrey", Password = "1488", LastName = "Petrov", Role = roles[1] },
                new User { FirstName = "Oleg", Password = "8814", LastName = "Kotlov", Role = roles[2] },
            };

            builder.Entity<Role>().HasData(roles);

            builder.Entity<User>().HasData(users);

            var students = new List<Student>
            {
                new Student { UserId = users[1].Id },
            };

            var teachers = new List<Teacher>
            {
                new Teacher { UserId = users[2].Id },
            };

            builder.Entity<Student>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            builder.Entity<Student>()
                .HasData(students);

            builder.Entity<Teacher>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            builder.Entity<Teacher>()
                .HasData(students);

            var studentTeachers = new List<StudentTeacher>()
            {
                new StudentTeacher { StudentId = students[0].Id, TeacherId = teachers[0].Id, Grade = 4 }
            };

            builder.Entity<StudentTeacher>()
                .HasKey(t => new { t.StudentId, t.TeacherId });

            builder.Entity<StudentTeacher>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentTeachers)
                .HasForeignKey(sc => sc.StudentId);

            builder.Entity<StudentTeacher>()
                .HasOne(sc => sc.Teacher)
                .WithMany(c => c.StudentTeachers)
                .HasForeignKey(sc => sc.StudentId);

            builder.Entity<StudentTeacher>()
                .HasData(studentTeachers);
        }
    }
}
