using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;

namespace Students_Site.BLL.Interfaces
{
    public interface ITeacherService: IService
    {
        void MakeTeacher(TeacherBLL teacherBll);
        TeacherBLL GetTeacher(int? id);
        void UpdateTeacher(TeacherBLL teacherBll);
        IEnumerable<TeacherBLL> GetTeachers();
    }
}
