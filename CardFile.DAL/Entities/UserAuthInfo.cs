using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Entities
{
    /// <summary>
    /// Сущность для передачи данных об аутентификации пользователя
    /// </summary>
    public class UserAuthInfo
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
