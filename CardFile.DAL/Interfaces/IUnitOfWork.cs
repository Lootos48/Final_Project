using CardFile.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Author> Authors { get; }
        IRepository<Card> Cards { get; }
    }
}
