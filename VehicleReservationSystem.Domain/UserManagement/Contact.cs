using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleReservationSystem.Domain.UserManagement
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName="varchar")]
        [MaxLength(20)]
        public string Name  { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        public string Email { get; set; }
        [Required]
        public string  Subject { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(1000)]
        public string Message { get; set; }
    }
}
