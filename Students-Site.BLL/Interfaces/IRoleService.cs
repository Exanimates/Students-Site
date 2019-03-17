using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;

namespace Students_Site.BLL.Interfaces
{
    public interface IRoleService: IService
    {
        void MakeRole(RoleBLL roleBll);
        RoleBLL GetRole(int? id);
    }
}
