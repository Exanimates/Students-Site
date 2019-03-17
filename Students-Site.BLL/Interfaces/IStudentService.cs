using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;

namespace Students_Site.BLL.Interfaces
{
    public interface IStudentService : IService
    {
        void MakeStudent(StudentBLL userBll);
        StudentBLL GetStudent(int? id);
        void UpdateStudent(StudentBLL teacherBll);
        IEnumerable<StudentBLL> GetStudents();
    }
}
