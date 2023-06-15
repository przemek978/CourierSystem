using CourierSystem.Models;
<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Identity;
>>>>>>> b062582e0d771dcdbe6c917fe1c426fb9889e72b
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourierSystem.Data
{
    public static class DB
    {
        private static DBContext _context;
        private const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=CourierDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


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

        public static void AddShipment(Shipment shipment)
        {
            _context.Add(shipment);
            _context.SaveChanges();
        }

        public static List<Shipment> GetShipmentsWithOtherTables()
        {
            return _context.Shipments.Include(p => p.Sender).Include(r => r.Recipient).Include(r => r.Status).Include(r => r.Courier).ToList();
        }

        public static void EditShipment(Shipment shipment)
        {
            _context.Attach(shipment).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (SearchShipment(shipment.ShipmentNumber) == null)
                {
                    MessageBox.Show("Nie znaleziono zamówienia");
                }
                else
                {
                    throw;
                }
            }

        }


        public static List<Shipment> GetShipmentsWithOtherTables()
        {
            return _context.Shipments.Include(p => p.Sender).Include(r => r.Recipient).Include(r => r.Status).Include(r => r.Courier).ToList();
        }

        public static void DeleteShipment(Shipment shipment)
        {
            _context.Shipments.Remove(shipment);
            _context.SaveChanges();
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

        public static void SendMessage(Message message)
        {
            _context.Messages.Add(message);

        public static List<ShipmentStatus> GetStatuses()
        {
            return _context.Statuses.ToList();
        }

        public static List<Courier> GetCouriers()
        {
            return _context.Couriers.ToList();
        }

        public static List<Person> GetPeople()
        {
            return _context.People.ToList();
        }

        public static Person SearchPerson(int number)
        {
            return _context.People.FirstOrDefault(p => p.PhoneNumber == number);
        }

        public static void AddPerson(Person person)
        {
            _context.Add(person);
            _context.SaveChanges();
        }

        private static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
