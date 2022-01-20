using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    /// <summary>
    /// Модель для вывода общей информации о пользователе
    /// </summary>
    public class UserInfoViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// Все роли пользователя
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Строка с ролями пользователя
        /// </summary>
        public string RolesStr
        {
            get
            {
                return string.Join(", ", Roles);
            }
        }
    }
}