using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Entities
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Title size must be between 4 and 100 characters")]
        public string Title { get; set; }

        [Required]
        public DateTime DateOfCreate { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 251, ErrorMessage = "Text minimum size is 50 characters")]
        public string Text { get; set; }

        [DefaultValue(0)]
        public int LikeAmount { get; set; }
        
        public virtual Author Author { get; set; }
    }
}
