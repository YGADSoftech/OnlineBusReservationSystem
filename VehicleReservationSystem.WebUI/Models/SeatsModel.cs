using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleReservationSystem.WebUI.Models
{
    public class SeatsModel
    {
        public int Id { get; set; }
        public int SeatNumber { get; set; }
        public bool IsAvail { get; set; }
        public string  VehicleName { get; set; }
    }
}