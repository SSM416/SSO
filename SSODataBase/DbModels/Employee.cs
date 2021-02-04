using System;
using System.Collections.Generic;

namespace SSODataBase.DbModels
{
    public partial class Employee
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int? UserSex { get; set; }
        public string UserPhone { get; set; }
        public int? UserLogins { get; set; }
        public string UserPwd { get; set; }
        public string UserEncryption { get; set; }
        public int? ValidFlag { get; set; }
    }
}
