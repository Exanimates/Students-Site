using System;
using System.Collections.Generic;
using Students_Site.DAL.Entities;
using System.Linq;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface ISubjectService : IDisposable
    {
        void Create(SubjectBLL subjectBll);
        SubjectBLL Get(int id);
        IEnumerable<SubjectBLL> GetAll();
    }

    public class SubjectService: ISubjectService
    {
        private IUnitOfWork _unitOfWork { get; }

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(SubjectBLL subjectBll)
        {
            var subjectByName = _unitOfWork.SubjectRepository.GetAll().FirstOrDefault(r => r.Name == subjectBll.Name);

            if (subjectByName != null)
                throw new ValidationException("Такой предмет уже существует", "");

            var subject = new Subject
            {
                Name = subjectBll.Name
            };

            _unitOfWork.SubjectRepository.Create(subject);

            _unitOfWork.Save();
        }

        public SubjectBLL Get(int id)
        {
            var subject = _unitOfWork.SubjectRepository.Get(id);

            if (subject == null)
                throw new ValidationException("Предмет не найден", "");

            return new SubjectBLL { Name = subject.Name };
        }

        public IEnumerable<SubjectBLL> GetAll()
        {
            return _unitOfWork.SubjectRepository.GetAll().Select(subject => new SubjectBLL
            {
                Id = subject.Id,
                Name = subject.Name
            }).ToArray();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}