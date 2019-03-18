using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;

namespace Students_Site.BLL.Interfaces
{
    public interface ITeacherService : IService
    {
        void MakeTeacher(UserBLL userBll, IEnumerable<int> studentsId);
        void UpdateTeacher(UserBLL userBll, IEnumerable<int> studentsId);
        IEnumerable<TeacherBLL> GetTeachers();
    }
}
