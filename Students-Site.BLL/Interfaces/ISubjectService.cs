using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;

namespace Students_Site.BLL.Interfaces
{
    public interface ISubjectService : IService
    {
        void MakeSubject(SubjectBLL subjectBll);
        SubjectBLL GetSubject(int? id);
        IEnumerable<SubjectBLL> GetSubjects();
    }
}
