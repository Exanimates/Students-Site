using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface IUserService : IDisposable
    {
        void Create(UserBLL userBll, IEnumerable<int> studentsId);
        void Update(UserBLL userBll, IEnumerable<int> studentsId);
        IEnumerable<UserBLL> GetAll();
    }
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork { get; }

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(UserBLL userBll, IEnumerable<int> studentsId)
        {
            var user = new User
            {
                FirstName = userBll.FirstName,
                LastName = userBll.LastName,
                Login = userBll.Login,
                Password = userBll.Password,
                Role = userBll.Role
            };

            _unitOfWork.UserRepository.Create(user);

            _unitOfWork.Save();
        }

        public IEnumerable<UserBLL> GetAll()
        {
            return _unitOfWork.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,

                Role = user.Role,

                Password = user.Password,
                Salt = user.Salt,
            }).ToArray();
        }

        public void Update(UserBLL userBll, IEnumerable<int> teachersId)
        {
            var user = _unitOfWork.UserRepository.Get(userBll.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.Role = userBll.Role;
            user.FirstName = userBll.FirstName;
            user.LastName = userBll.LastName;
            user.Login = userBll.Login;
            user.Password = userBll.Password;

            _unitOfWork.UserRepository.Update(user);

            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}