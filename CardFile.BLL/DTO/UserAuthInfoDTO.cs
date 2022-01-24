using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.DTO
{
    /// <summary>
    /// DTO для данных аутентификации пользователя
    /// </summary>
    public class UserAuthInfoDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
