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
    public interface IRoleService : IDisposable
    {
        void Create(RoleBLL roleBll);
        RoleBLL Get(int id);
    }

    public class RoleService: IRoleService
    {
        private IUnitOfWork _unitOfWork { get; }

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(RoleBLL roleBll)
        {
            var roleByName = _unitOfWork.RoleRepository.GetAll().FirstOrDefault(r => r.Name == roleBll.Name);

            if (roleByName != null)
                throw new ValidationException("Такая роль уже существует", "");

            var role  = new Role
            {
                Name = roleBll.Name
            };
            _unitOfWork.RoleRepository.Create(role);
            _unitOfWork.Save();
        }

        public RoleBLL Get(int id)
        {
            var role = _unitOfWork.RoleRepository.Get(id);

            if (role == null)
                throw new ValidationException("Роль не найдена", "");

            return new RoleBLL { Name = role.Name };
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}