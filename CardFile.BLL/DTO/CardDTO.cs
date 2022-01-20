using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.DTO
{
    /// <summary>
    /// DTO сущности карточки
    /// </summary>
    public class CardDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateOfCreate { get; set; }

        public string Text { get; set; }

        public int LikeAmount { get; set; }

        public int? AuthorId { get; set; }
        public AuthorDTO Author { get; set; }
    }
}
