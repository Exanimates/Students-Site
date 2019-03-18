using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Students_Site.DAL.EF;
using Students_Site.DAL.Interfaces;
using Students_Site.DAL.UnitOfWork;

namespace Students_Site.BLL.Infrastructure
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                var options = new DbContextOptionsBuilder<ApplicationContext>();
                options.UseSqlServer(config.GetSection("ConnectionString:DefaultConnection").Value);

                return new ApplicationContext(options.Options);
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
