using CardFile.BLL.Interfaces;
using CardFile.BLL.Services;
using CardFile.DAL.EF;
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
    /// Ninject-модуль для привязки всех классов репозиториев к их интерфейсам
    /// </summary>
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            var context = new CardFileContext();
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument("cardFileContext", context);
            Bind<ILikeService>().To<LikeService>();
            Bind<IIdentityProvider>().To<IdentityProvider>();
            Bind<IAuthorsService>().To<AuthorService>();
            Bind<ICardsService>().To<CardsService>();
            Bind<IIdentityService>().To<IdentityService>();
        }
    }
}
