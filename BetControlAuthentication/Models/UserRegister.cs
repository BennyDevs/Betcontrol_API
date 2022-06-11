using System;
using System.ComponentModel.DataAnnotations;

namespace BetControlAuthentication.Models
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; } = String.Empty;
        public string? Username { get; set; }
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = String.Empty;
        [Compare("Password", ErrorMessage = "Passwords need to match.")]
        public string ConfirmPassword { get; set; } = String.Empty;
        [Range(typeof(bool), "true", "true", ErrorMessage = "You need to confirm T&S.")]
        public bool IsConfirmed { get; set; }
    }
}

