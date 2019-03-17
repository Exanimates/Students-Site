using Students_Site.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Students_Site.BLL.Interfaces
{
    public interface IService
    {
        IUnitOfWork Database { get; set; }
        void Dispose();
    }
}
