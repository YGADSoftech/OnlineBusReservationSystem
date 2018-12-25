using System;
using System.Collections.Generic;

namespace VehicleReservationSystem.Domain.VehicleManagement
{
    public class Booking
    {

        public Booking()
        {
            Seats = new List<Seat>();
        }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string Phone { get; set; }
        public string IdCard { get; set; }

        public string email { get; set; }
        public bool Is_Paid { get; set; }
        public bool Is_Admin_Seen { get; set; }

        public virtual List<Seat> Seats { get; set; }
        public virtual TimeTable TimeTables { get; set; }

        public float TotalPrice { get; set; }

    }
}
