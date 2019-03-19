using System.Collections.Generic;
using System.Linq;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface IUserService : IService
    {
        void MakeUser(UserBLL userBll, IEnumerable<int> studentsId);
        void UpdateUser(UserBLL userBll, IEnumerable<int> studentsId);
        IEnumerable<UserBLL> GetUsers();
    }
    public class UserService : IUserService
    {
        IUnitOfWork _database { get; set; }

        public UserService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }

        public void MakeUser(UserBLL userBll, IEnumerable<int> studentsId)
        {
            var user = new User
            {
                FirstName = userBll.FirstName,
                LastName = userBll.LastName,
                Login = userBll.Login,
                Password = userBll.Password,
                RoleId = userBll.RoleId
            };

            _database.UserRepository.Create(user);

            _database.Save();
        }

        public IEnumerable<UserBLL> GetUsers()
        {
            return _database.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                RoleId = user.RoleId
            }).ToArray();
        }

        public void UpdateUser(UserBLL userBll, IEnumerable<int> teachersId)
        {
            var user = _database.UserRepository.Get(userBll.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.RoleId = userBll.RoleId;
            user.FirstName = userBll.FirstName;
            user.LastName = userBll.LastName;
            user.Login = userBll.Login;
            user.Password = userBll.Password;

            _database.UserRepository.Update(user);

            _database.Save();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
