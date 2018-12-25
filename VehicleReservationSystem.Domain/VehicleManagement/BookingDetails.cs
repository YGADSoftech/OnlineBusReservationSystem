using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleReservationSystem.Domain.VehicleManagement
{
    public class BookingDetails
    {
        public int Id { get; set; }
        public int NumberOfSeats { get; set; }
        public float Price { get; set; }
        
        public virtual Booking Booking { get; set; }
        public virtual TimeTable TimeTable { get; set; }
        public bool Is_Admin_Seen { get; set; }

       // public virtual ICollection<Seat> SeatsOffered { get; set; }
    }
}
