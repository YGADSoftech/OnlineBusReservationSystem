using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleReservationSystem.Domain.UserManagement
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string LoginId { get; set; }


        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; }



        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string FistName { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string LastName { get; set; }

        [MaxLength(300)]
        [Column(TypeName = "varchar")]
        public string ImageUrl { get; set; }


        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Email { get; set; }


        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }



        public bool Is_Active { get; set; }
        public virtual RememberPassword RememberPassword { get; set; }



    }
}
