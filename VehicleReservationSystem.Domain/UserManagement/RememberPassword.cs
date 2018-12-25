using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleReservationSystem.Domain.UserManagement
{
   public class RememberPassword
    {
        public int Id { get; set; }
        
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string RememberToken { get; set; }
    }
}
