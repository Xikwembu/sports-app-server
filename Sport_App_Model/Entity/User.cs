using System;
using System.ComponentModel.DataAnnotations;

namespace Sport_App_Model.Entity
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string IdNumber { get; set; }
        [Required]
        public string Race { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
