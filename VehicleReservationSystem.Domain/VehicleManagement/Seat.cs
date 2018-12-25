using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleReservationSystem.Domain.VehicleManagement
{
    public class Seat
    {
        public int Id { get; set; }
        [Required]
        public int SeatNumber { get; set; }

        [Required]
        public bool Is_Available { get; set; }

        [Required]
        public virtual Vehicle Vehicle { get; set; }
    }
}
