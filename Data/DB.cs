using CourierSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CourierSystem.Data
{
    public static class DB
    {
        private static DBContext _context;
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


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

        public static Shipment SearchShipment(long Number)
        {
            return _context.Shipments.FirstOrDefault(s => s.ShipmentNumber == Number);

        }

        public static List<Shipment> GetShipmentsWithOtherTables()
        {
            return _context.Shipments.Include(p => p.Sender).Include(r => r.Recipient).Include(r => r.Status).Include(r => r.Courier).ToList();
        }

        public static ShipmentStatus SearchStatus(Shipment shipment)
        {
            return _context.Statuses.FirstOrDefault(s => s.Id == shipment.StatusId);
        }

        public static User ValidateUser(string username,string password)
        {
            var hasher = new PasswordHasher<User>();
            var user= _context.Users.FirstOrDefault(u=> u.Username == username);

            if (user == null)
            {
                return null;
            }
            var passwordValid = hasher.VerifyHashedPassword(null, user.Password, password) == PasswordVerificationResult.Success;

            if (!passwordValid)
            {
                return null;
            }

            return user;
        }

        private static string GetConnectionString()
        {
            return ConnectionString;
        }
    }

}
