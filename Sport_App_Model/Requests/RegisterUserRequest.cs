using System.ComponentModel.DataAnnotations;

namespace Sport_App_Model.Requests
{
    public class RegisterUserRequest
    {
        public string? Name { get; set; }
        
        public string? Surname { get; set; }
        
        public string? Password { get; set; }
        
        public string? Email { get; set; }
        
        public string IdNumber { get; set; }
       
        public string? Race { get; set; }
        
        public string Role { get; set; }
    }
}
