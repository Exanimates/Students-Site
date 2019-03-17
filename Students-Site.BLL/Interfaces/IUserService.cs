    using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;

namespace Students_Site.BLL.Interfaces
{
    public interface IUserService : IService
    {
        void MakeUser(UserBLL userBll);
        UserBLL GetUser(int? id);
        UserBLL UpdateUser(UserBLL userBll);
        IEnumerable<UserBLL> GetUsers();
    }
}
