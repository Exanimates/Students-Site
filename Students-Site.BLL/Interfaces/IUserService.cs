using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;

namespace Students_Site.BLL.Interfaces
{
    public interface IUserService
    {
        void MakeUser(UserBLL orderDto);
        UserBLL GetUser(int? id);
        IEnumerable<UserBLL> GetUsers();
        IEnumerable<StudentBLL> GetStudents();
        IEnumerable<TeacherBLL> GetTeachers();
        void Dispose();
    }
}
