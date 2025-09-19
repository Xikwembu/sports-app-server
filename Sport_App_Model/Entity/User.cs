using System.ComponentModel.DataAnnotations;
using Sports_App_Model.Entity;

namespace Sport_App_Model.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Surname { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string IdNumber { get; set; } = null!;

        [Required]
        public string Race { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        [Required]
        public string RoleType { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public LoginOtp? LoginOtp { get; set; }
    }
}
