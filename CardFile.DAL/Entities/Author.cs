using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3, ErrorMessage = "First name size must be between 3 and 60 characters")]
        public string FirstName { get; set; }

        [StringLength(60, MinimumLength = 3, ErrorMessage = "Second name size must be between 3 and 60 characters")]
        public string SecondName { get; set; }

        public virtual ICollection<Card> Cards { get; set; }

        public Author()
        {
            Cards = new List<Card>();
        }
    }
}
