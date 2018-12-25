using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VehicleReservationSystem.Domain.VehicleManagement;

namespace VehicleReservationSystem.WebUI.Models
{
    public class RouteModel
    {
        public int Id { get; set; }
       
        public float Price { get; set; }
        public TimeSpan TravelTime { get; set; }
        public string OriginName { get; set; }
        public string DestName { get; set; }
        public string vehicleType { get; set; }

       
        
    }
}