using Sport_App_Model.Returns;
using Sport_App_Service.Encryption;
using Sport_App_Service.Token;
using Sports_App_Repository.UserRepository;

namespace Sport_App_Service.Auth.Login
{
    public class LoginService : ILoginService
    {
        public readonly IUserRepository _userRepository;
        public readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;

        public LoginService(IUserRepository userRepository, ITokenService tokenService, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _encryptionService = encryptionService;
        }

        public AuthReturn LoginReturn(string email, string password)
        {
            var existingUser = _userRepository.GetUserByEmailAsync(email).Result;

            if (existingUser == null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Messsage = "Invalid credentials"
                };
            }

            bool isValid = _encryptionService.VerifyPassword(password, existingUser.Password);

            if (!isValid)
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
