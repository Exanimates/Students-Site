using Students_Site.DAL.EF;
using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class StudentTeacherRepository : BaseRepository<StudentTeacher>
    {
        public StudentTeacherRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
