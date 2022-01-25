using System;
using System.ComponentModel.DataAnnotations;

namespace BetControlAuthentication.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter an email address.")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

