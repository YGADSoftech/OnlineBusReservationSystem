using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleReservationSystem.WebUI.Models
{
    public class VehiclesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string  RegistrationNumber { get; set; }
        public int Total_Seats { get; set; }
        public string  VehicleType { get; set; }


    }
}