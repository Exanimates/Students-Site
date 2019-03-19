using Students_Site.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;
using Students_Site.BLL.Helpers;
using Students_Site.DAL.Entities;
using System.Linq;
using Students_Site.BLL.Interfaces;
using Students_Site.DAL.UnitOfWork;

namespace Students_Site.BLL.Services
{
    public interface ISubjectService : IService
    {
        void MakeSubject(SubjectBLL subjectBll);
        SubjectBLL GetSubject(int? id);
        IEnumerable<SubjectBLL> GetSubjects();
    }

    public class SubjectService: ISubjectService
    {
        IUnitOfWork Database { get; set; }

        public SubjectService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public void MakeSubject(SubjectBLL subjectBll)
        {
            var subjectByName = Database.SubjectRepository.GetAll().FirstOrDefault(r => r.Name == subjectBll.Name);

            if (subjectByName != null)
                throw new ValidationException("Такой предмет уже существует", "");

            var subject = new Subject
            {
                Name = subjectBll.Name
            };

            Database.SubjectRepository.Create(subject);

            Database.Save();
        }

        public SubjectBLL GetSubject(int? id)
        {
            if (id == null)
                throw new ValidationException("Id предмета не установлено", "");

            var role = Database.RoleRepository.Get(id.Value);

            if (role == null)
                throw new ValidationException("Предмет не найден", "");

            return new SubjectBLL { Name = role.Name };
        }

        public IEnumerable<SubjectBLL> GetSubjects()
        {
            return Database.SubjectRepository.GetAll().Select(subject => new SubjectBLL
            {
                Name = subject.Name
            });
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
