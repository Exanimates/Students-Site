
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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
                new Role { Id = 1, Name = "Dean" },
                new Role { Id = 2, Name = "Student" },
                new Role { Id = 3, Name = "Teacher" },
            };

            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Petr", Password = "123", LastName = "Ivanov", RoleId = roles[0].Id },
                new User { Id = 2, FirstName = "Andrey", Password = "1488", LastName = "Petrov", RoleId = roles[1].Id },
                new User { Id = 3, FirstName = "Oleg", Password = "8814", LastName = "Kotlov", RoleId = roles[2].Id },
            };

            builder.Entity<Role>().HasData(roles);

            builder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId);

            builder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Entity<User>()
                .HasOne(u => u.Teacher)
                .WithOne(s => s.User)
                .HasForeignKey<Teacher>(s => s.UserId);

            builder.Entity<User>()
                .HasOne(u => u.Teacher)
                .WithOne(s => s.User)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Entity<User>().HasData(users);                  

            var subjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "История" },
            };

            builder.Entity<Subject>().HasData(subjects);            

            builder.Entity<Student>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            builder.Entity<Teacher>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            builder.Entity<StudentTeacher>()
                .HasKey(t => new { t.StudentId, t.TeacherId });

            builder.Entity<StudentTeacher>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentTeachers)
                .HasForeignKey(sc => sc.StudentId);

            builder.Entity<StudentTeacher>()
                .HasOne(sc => sc.Teacher)
                .WithMany(c => c.StudentTeachers)
                .HasForeignKey(sc => sc.TeacherId);

            var teachers = new List<Teacher>
            {
                new Teacher { TeacherId = 1, UserId = users[2].Id, SubjectId = subjects[0].Id},
            };
            builder.Entity<Teacher>().HasData(teachers);

            var students = new List<Student>
            {
                new Student { StudentId = 1, UserId = users[1].Id }
            };
            builder.Entity<Student>().HasData(students);

            var studentTeachers = new List<StudentTeacher>
            {
                new StudentTeacher { StudentId = students[0].StudentId, TeacherId = teachers[0].TeacherId },
            };
            builder.Entity<StudentTeacher>().HasData(studentTeachers);
        }
    }
}
