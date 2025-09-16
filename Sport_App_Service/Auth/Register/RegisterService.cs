using Sport_App_Model.Entity;
using Sport_App_Model.Returns;
using Sports_App_Repository.UserRepository;

namespace Sport_App_Service.Auth.Register
{
    public class RegisterService : IRegisterService
    {
        public readonly IUserRepository _userRepository;

        public RegisterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AuthReturn RegisterReturn(string email, string password, string role)
        {
            var existingUser = _userRepository.GetUserByEmailAsync(email).Result;

            if (existingUser != null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Messsage = "Email already in use"
                };
            }

            var newUser = new User
            {
                Email = email,
                Password = password,
                Role = role,
            };

            _userRepository.AddUserAsync(newUser).Wait();

            return new AuthReturn
            {
                Status = true,
                Messsage = "User registered successfully"
            };
        }
    }
}
