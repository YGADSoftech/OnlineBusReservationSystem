using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleReservationSystem.Domain.VehicleManagement
{
    public class Route:IListable
    {

        public int Id { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public TimeSpan TravelTime { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual City  Destination { get; set; }
       
        public virtual City Origin { get; set; }

       
    
    }
}
