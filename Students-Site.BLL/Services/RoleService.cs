using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;
using Students_Site.BLL.Helpers;
using Students_Site.BLL.Interfaces;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Interfaces;
using Students_Site.DAL.UnitOfWork;

namespace Students_Site.BLL.Services
{
    public interface IRoleService : IService
    {
        void MakeRole(RoleBLL roleBll);
        RoleBLL GetRole(int? id);
    }

    public class RoleService: IRoleService
    {
        IUnitOfWork _database { get; set; }

        public RoleService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }

        public void MakeRole(RoleBLL roleBll)
        {
            Role roleByName = _database.RoleRepository.GetAll().FirstOrDefault(r => r.Name == roleBll.Name);

            if (roleByName != null)
                throw new ValidationException("Такая роль уже существует", "");

            Role role  = new Role
            {
                Name = roleBll.Name
            };
            _database.RoleRepository.Create(role);
            _database.Save();
        }

        public RoleBLL GetRole(int? id)
        {
            if (id == null)
                throw new ValidationException("Id роли не установлено", "");
            var role = _database.RoleRepository.Get(id.Value);
            if (role == null)
                throw new ValidationException("Роль не найдена", "");

            return new RoleBLL { Name = role.Name };
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
