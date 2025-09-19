namespace Sports_App_Model.Requests.Auth
{
    public class RegisterUserRequest
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public string Race { get; set; }
        public string Role { get; set; }
        public string RoleType { get; set; }
    }
}
