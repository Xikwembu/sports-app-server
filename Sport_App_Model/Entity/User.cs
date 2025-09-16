using System;
using System.ComponentModel.DataAnnotations;

namespace Sport_App_Model.Entity
{
    public class User
    {
        [Key]
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
