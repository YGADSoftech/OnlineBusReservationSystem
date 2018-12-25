using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleReservationSystem.Domain.UserManagement;
using VehicleReservationSystem.Domain.VehicleManagement;

namespace VehicleReservationSystem.Domain
{
    public class VehiclesContext : DbContext
    {
        public VehiclesContext() : base("VehicleStr")
        {
            this.Configuration.LazyLoadingEnabled = false;

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

        }

        public DbSet<User> Users { get; set; }
        public DbSet<RememberPassword> RememberPasswords { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Seat> Seats { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Booking> BookingDetails { get; set; }
    }

}
