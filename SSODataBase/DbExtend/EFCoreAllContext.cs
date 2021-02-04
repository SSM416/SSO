using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSODataBase.DbExtend
{
    public class EFCoreAllContext : DbContext
    {
        private readonly string strConn = string.Empty;

        public EFCoreAllContext(string conn)
        {
            strConn = conn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(strConn);
        }
    }
}
