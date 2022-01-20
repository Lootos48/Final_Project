using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.DTO
{
    /// <summary>
    /// DTO для получения и передачи Никнейма пользователя и его ролей
    /// </summary>
    public class UserInfoDTO
    {
        public string Username { get; set; }
        public string[] Roles { get; set; }
    }
}
