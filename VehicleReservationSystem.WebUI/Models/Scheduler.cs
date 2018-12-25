using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleReservationSystem.WebUI.Models
{
    public class Scheduler
    {
        public int OriginID { get; set; }
        public int DestID { get; set; }
        public string Date { get; set; }
       
    }
}