using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierSystem.Data;
using Microsoft.AspNetCore.Identity;

namespace CourierSystem.Models
{
    public class Courier:User
    {
        public string Name { get; set; }
        //public string Username { get; set; }
        //public string Password { get; set; }
        //public string Role { get; set; }

        public ICollection<Shipment> Shipments { get; set; }

        public Courier(string username, string password, string role, string name) 
        {
            Name = name;
            Password = password;
            Role = role;
            Username=username;
        }
        public string PasswordHash()
        {
            var hasher = new PasswordHasher<Courier>();
            Password = hasher.HashPassword(this, Password);
            return Password;
        }
    }
}
