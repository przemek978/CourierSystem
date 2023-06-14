﻿using CourierSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
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

        public static ShipmentStatus SearchStatus(Shipment shipment)
        {
            return _context.Statuses.FirstOrDefault(s => s.Id == shipment.StatusId);
        }

        public static List<Courier> GetCouriers()
        {
            return _context.Couriers.ToList();
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

        public static void AddShipment(Shipment shipment)
        {
            _context.Add(shipment);
            _context.SaveChanges();
        }

        private static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
