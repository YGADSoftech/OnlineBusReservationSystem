using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleReservationSystem.Domain.VehicleManagement
{
    public class TimeTable
    {
        public int Id { get; set; }

        [Required]
        public TimeSpan DepartureTime { get; set; }
        

        public Nullable<DateTime> Date { get; set; }
        public bool IsFirst { get; set; }

    
        public virtual Vehicle Vehicle { get; set; }
        public virtual Route Route { get; set; }
    }
}
