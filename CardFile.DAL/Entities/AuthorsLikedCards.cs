using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardFile.DAL.Entities
{
    public class AuthorsLikedCards
    {
        [Key]
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        [Key]
        [ForeignKey(nameof(Card))]
        public int CardId { get; set; }
        public virtual Author Author { get; set; }
        public virtual Card Card { get; set; }
    }
}
