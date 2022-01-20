using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    /// <summary>
    /// Модель для данных аутентификации пользователя
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "User name size must be between 3 and 40 characters")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "User name size must be between 5 and 100 characters")]
        public string Password { get; set; }
    }
}