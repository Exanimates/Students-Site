using Autofac;
using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.EF;
using Students_Site.DAL.Interfaces;
using Students_Site.DAL.UnitOfWork;

namespace Students_Site.BLL.Infrastructure
{
    public class ServiceModule : Module
    {
        private DbContextOptions<ApplicationContext> _options;
        public ServiceModule(DbContextOptions<ApplicationContext> option)
        {
            _options = option;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationContext>().AsSelf();
        }
    }
}
