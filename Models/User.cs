using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CourierSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }    

        public User()
        {

        }
        public User(string username, string password,string role)
        {
            Username = username;
            Password = password;
            Role=role;  
        }
        public string PasswordHash()
        {
            var hasher = new PasswordHasher<User>();
            Password = hasher.HashPassword(this, Password);
            return Password;
        }



    }
}
