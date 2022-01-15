using CardFile.BLL.Interfaces;
using CardFile.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    public class ProcedureService
    {
        public static async Task<int> AddAuthorToCard(int cardId, int AuthorId)
        {
            int result;
            using (CardFileContext context = new CardFileContext("CardFileDBConnection"))
            {
                result = await context.AddAuthorToCard(cardId, AuthorId);
            }
            return result;
        }
    }
}
