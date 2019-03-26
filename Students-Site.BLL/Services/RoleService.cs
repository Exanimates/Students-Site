﻿using System;
using System.Linq;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface IRoleService : IDisposable
    {
        void MakeRole(RoleBLL roleBll);
        RoleBLL GetRole(int id);
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
            var roleByName = _database.RoleRepository.GetAll().FirstOrDefault(r => r.Name == roleBll.Name);

            if (roleByName != null)
                throw new ValidationException("Такая роль уже существует", "");

            var role  = new Role
            {
                Name = roleBll.Name
            };
            _database.RoleRepository.Create(role);
            _database.Save();
        }

        public RoleBLL GetRole(int id)
        {
            var role = _database.RoleRepository.Get(id);

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