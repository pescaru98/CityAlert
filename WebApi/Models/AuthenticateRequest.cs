using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Models
{
    public class AuthenticateRequest
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
