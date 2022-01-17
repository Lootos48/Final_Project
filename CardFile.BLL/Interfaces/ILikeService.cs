using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    public interface ILikeService
    {
        bool IsAuthorAlreadyLikeCard(int cardId, int authorId);
        Task<bool> LikeCard(int cardId, int authorId);
    }
}
