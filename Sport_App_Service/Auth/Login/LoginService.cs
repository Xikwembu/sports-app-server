using Sport_App_Model.Returns;
using Sport_App_Service.Token;
using Sports_App_Repository.UserRepository;

namespace Sport_App_Service.Auth.Login
{
    public class LoginService : ILoginService
    {
        public readonly IUserRepository _userRepository;
        public readonly ITokenService _tokenService;

        public LoginService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public AuthReturn LoginReturn(string email, string password)
        {
            var existingUser = _userRepository.GetUserByEmailAsync(email).Result;

            if (existingUser == null || existingUser.Password != password)
            {
                return new AuthReturn
                {
                    Status = false,
                    Messsage = "Invalid credentials"
                };
            }

            var token = _tokenService.GenerateToken(existingUser.Email, existingUser.Role);

            return new AuthReturn
            {
                Status = true,
                Messsage = "Login successful",
                Token = token
            };
        }
    }
}
