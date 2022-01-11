using CardFile.BLL.Interfaces;
using CardFile.BLL.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Util
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthorsService>().To<AuthorService>();
            Bind<ICardsService>().To<CardsService>();
            Bind<IIdentityService>().To<IdentityService>();
        }
    }
}