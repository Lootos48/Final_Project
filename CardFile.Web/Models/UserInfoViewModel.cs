using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    public class UserInfoViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }

        public string RolesStr
        {
            get
            {
                return string.Join(", ", Roles);
            }
        }
    }
}