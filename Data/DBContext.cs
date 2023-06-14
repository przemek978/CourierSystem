using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CourierSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;

namespace CourierSystem.Data
{

    public class DBContext : DbContext
    {
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<ShipmentStatus> Statuses { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            try
            {
                Database.EnsureCreated();
                if (!Couriers.Any() && !People.Any() && !Statuses.Any())
                {
                    Couriers.Add(new Courier { Name = "Andrzej" });
                    Couriers.Add(new Courier { Name = "Janusz" });
                    Couriers.Add(new Courier { Name = "Marek" });

                    People.Add(new Person { FirstName = "Marcin", LastName = "Kaczanowski", Address = "Wiejska 45A, 15-351 Białystok", PhoneNumber = 321654987 });
                    People.Add(new Person { FirstName = "Przemysław", LastName = "Kuczyński", Address = "Zwierzyniecka 7, 15-312 Białystok", PhoneNumber = 654987321 });
                    People.Add(new Person { FirstName = "Jan", LastName = "Jelski", Address = "Wierzbowa 5, 15-743 Białystok", PhoneNumber = 654321987 });

                    Statuses.Add(new ShipmentStatus { Status="Przygotowanie do nadania"});
                    Statuses.Add(new ShipmentStatus { Status="Odebrana przez kuriera"});
                    Statuses.Add(new ShipmentStatus { Status="W drodze"});
                    Statuses.Add(new ShipmentStatus { Status="Przyjęta w sortowni"});
                    Statuses.Add(new ShipmentStatus { Status="Wysłana z sortowni"});
                    Statuses.Add(new ShipmentStatus { Status="Przyjęta w oddziale"});
                    Statuses.Add(new ShipmentStatus { Status="Wydana do doręczenia"});
                    Statuses.Add(new ShipmentStatus { Status="Doręczona"});
                    Statuses.Add(new ShipmentStatus { Status="Odbiorca niedostępny. Próba kolejnego dnia"});
                    Statuses.Add(new ShipmentStatus { Status="Awizo do odbioru w punkcie"});
                    SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=(localdb)\\ProjektDB;Initial Catalog=CourierDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Sender)
                .WithMany(p => p.SentShipments)
                .HasForeignKey(s => s.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Recipient)
                .WithMany(p => p.ReceivedShipments)
                .HasForeignKey(s => s.RecipientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shipment>()
                .Property(s => s.ShipmentNumber)
                .ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }

}
