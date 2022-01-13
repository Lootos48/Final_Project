using CardFile.DAL.Interfaces;
using CardFile.DAL.Repositories;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Infrastructure
{
    /// <summary>
    /// Ninject модуль для DI уровня DAL
    /// </summary>
    public class UoWModule : NinjectModule
    {
        private readonly string connectionString;

        /// <summary>
        /// Конструктор Ninject-модуля
        /// </summary>
        /// <param name="connection">Строка подключения к БД</param>
        public UoWModule(string connection)
        {
            connectionString = connection;
        }

        /// <summary>
        /// Метод привязки класса UnitOfWork к его интерфейсу и класса репозитория Identity к его интерфейса
        /// </summary>
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IIdentityProvider>().To<IdentityProvider>();
        }
    }
}
