using System.Collections.Generic;
using Common.Encryption;
using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.EF
{
    public class ApplicationContext : DbContext
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
                new Role { Id = 1, Name = "Декан" },
                new Role { Id = 2, Name = "Студент" },
                new Role { Id = 3, Name = "Учитель" }
            };

            var deanSalt = Salt.Create();
            var deanUser = new User
            {
                Id = 1,
                Login = "Dean",
                FirstName = "Petr",
                LastName = "Ivanov",
                RoleId = roles[0].Id,

                Salt = deanSalt,
                Password = Hash.Create("123", deanSalt),
            };

            var studentSalt = Salt.Create();
            var studentUser = new User
            {
                Id = 2,
                Login = "Student",
                FirstName = "Petr",
                LastName = "Ivanov",
                RoleId = roles[1].Id,

                Salt = studentSalt,
                Password = Hash.Create("1488", studentSalt),
            };

            var teacherSalt = Salt.Create();
            var teacherUser = new User
            {
                Id = 3,
                Login = "Teacher",
                FirstName = "Petr",
                LastName = "Ivanov",
                RoleId = roles[2].Id,

                Salt = teacherSalt,
                Password = Hash.Create("8814", teacherSalt),
            };

            var users = new List<User>
            {
                deanUser,
                studentUser,
                teacherUser
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

            builder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            builder.Entity<User>().HasData(users);

            var subjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "История" },
                new Subject {Id = 2, Name = "Информатика" }
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
                new Teacher { Id = 1, UserId = users[2].Id, SubjectId = subjects[0].Id},
            };
            builder.Entity<Teacher>().HasData(teachers);

            var students = new List<Student>
            {
                new Student { Id = 1, UserId = users[1].Id }
            };
            builder.Entity<Student>().HasData(students);

            var studentTeachers = new List<StudentTeacher>
            {
                new StudentTeacher { StudentId = students[0].Id, TeacherId = teachers[0].Id, Grade = 4 },
            };
            builder.Entity<StudentTeacher>().HasData(studentTeachers);
        }
    }
}