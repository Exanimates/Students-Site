using System.Collections.Generic;
using Students_Site.DAL.Entities;
using System.Linq;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Infrastructure;

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
        IUnitOfWork _database { get; set; }

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }

        public void MakeSubject(SubjectBLL subjectBll)
        {
            var subjectByName = _database.SubjectRepository.GetAll().FirstOrDefault(r => r.Name == subjectBll.Name);

            if (subjectByName != null)
                throw new ValidationException("Такой предмет уже существует", "");

            var subject = new Subject
            {
                Name = subjectBll.Name
            };

            _database.SubjectRepository.Create(subject);

            _database.Save();
        }

        public SubjectBLL GetSubject(int? id)
        {
            if (id == null)
                throw new ValidationException("Id предмета не установлено", "");

            var subject = _database.SubjectRepository.Get(id.Value);

            if (subject == null)
                throw new ValidationException("Предмет не найден", "");

            return new SubjectBLL { Name = subject.Name };
        }

        public IEnumerable<SubjectBLL> GetSubjects()
        {
            return _database.SubjectRepository.GetAll().Select(subject => new SubjectBLL
            {
                Id = subject.Id,
                Name = subject.Name
            }).ToArray();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}