using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public ICollection<CardDTO> Cards { get; set; }

        public AuthorDTO()
        {
            Cards = new List<CardDTO>();
        }
    }
}
