using System.ComponentModel.DataAnnotations;
using Sport_App_Model.Entity;

namespace Sports_App_Model.Entity
{
    public class LoginOtp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Otp { get; set; } = null!;

        [Required]
        public DateTime Expiry { get; set; }

        public User User { get; set; } = null!;
    }
}
