using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;

namespace Students_Site.BLL.Interfaces
{
    public interface IStudentService : IService
    {
        void MakeStudent(UserBLL userBll, IEnumerable<int> teachersId);
        void UpdateStudent(UserBLL userBll, IEnumerable<int> teachersId);
        IEnumerable<StudentBLL> GetStudents();
    }
}
