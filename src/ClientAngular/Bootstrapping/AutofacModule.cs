using Autofac;
using ClientAngular.Configuration;
using ClientAngular.Interfaces;
using ClientAngular.Repository;
using ClientAngular.Services;

namespace ClientAngular.Bootstrapping
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Configuration
            builder.RegisterType<DataConnectionProvider>().As<IDataConnection>();
            builder.RegisterType<DBConfig>().As<IDBConfig>().InstancePerLifetimeScope();

            //Repositories
            builder.RegisterType<OlympicWinnerRepository>().As<IOlympicWinnerRepository>().InstancePerLifetimeScope();

            //Services
            builder.RegisterType<OlympicWinnerService>().As<IOlympicWinnerService>().InstancePerLifetimeScope();
        }
    }
}
