using Sport_App_Model.Returns;
using Sport_App_Service.Encryption;
using Sports_App_Repository.UserRepository;
using Sports_App_Service.Auth.Otp;
using Sports_App_Service.Token.Otp;

namespace Sport_App_Service.Auth.Login
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpTokenService _otpTokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IOtpService _otpService;

        public LoginService(IUserRepository userRepository, IOtpTokenService otpTokenService,
            IEncryptionService encryptionService, IOtpService otpService)
        {
            _userRepository = userRepository;
            _otpTokenService = otpTokenService;
            _encryptionService = encryptionService;
            _otpService = otpService;
        }

        public async Task<AuthReturn> LoginUserAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);

            if (existingUser == null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Invalid credentials"
                };
            }

            bool isValid = _encryptionService.VerifyPassword(password, existingUser.Password);

            if (!isValid)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Invalid credentials"
                };
            }

            var otpToken = _otpTokenService.GenerateOtpToken(existingUser.Email);
            var result = await _otpService.StoreOtp(existingUser.Id);

            return new AuthReturn
            {
                Status = true,
                Message = result.Message,
                OtpToken = otpToken,
            };
        }
    }
}
