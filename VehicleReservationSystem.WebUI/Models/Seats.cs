using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleReservationSystem.WebUI.Models
{
    public class Seats
    {
        public Seats()
        {
            seats = new List<SeatsModel>();
        }
        public List<SeatsModel> seats { get; set; }
        
        
    }
}