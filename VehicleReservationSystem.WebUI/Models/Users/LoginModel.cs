using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VehicleReservationSystem.WebUI.Models.Users
{
    public class LoginModel
    {
        [Required]
        public string LoginId { get; set; }

        [Required]
        public string Password { get; set; }


        public bool RememberMe { get; set; }

    }
}