using System;
using System.Collections.Generic;

namespace SSODataBase.DbModels
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
