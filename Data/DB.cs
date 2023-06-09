using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierSystem.Data
{
    public static class DB
    {
        private static DBContext _context;
        private const string ConnectionString = @"Data Source=(localdb)\ProjektDB;Database=CourierDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public static DBContext GetInstance()
        {
            if (_context == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DBContext>();
                optionsBuilder.UseSqlServer(GetConnectionString());
                _context = new DBContext(optionsBuilder.Options);
            }

            return _context;
        }

        private static string GetConnectionString()
        {
            return ConnectionString;
        }
    }

}
