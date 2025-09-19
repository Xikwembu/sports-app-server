using Sport_App_Model.Returns;
using Sport_App_Service.Encryption;
using Sports_App_Model.Entity;
using Sports_App_Repository.OtpRepository;
using Sports_App_Repository.UserRepository;
using Sports_App_Service.Token.Auth;
using Sports_App_Service.Utilities;

namespace Sports_App_Service.Auth.Otp
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IAuthTokenService _authTokenService;
        private readonly IUserRepository _userRepository;

        public OtpService(IOtpRepository otpRepository, IEncryptionService encryptionService,
            IAuthTokenService authTokenService, IUserRepository userRepository)
        {
            _otpRepository = otpRepository;
            _encryptionService = encryptionService;
            _authTokenService = authTokenService;
            _userRepository = userRepository;
        }

        public async Task<AuthReturn> StoreOtp(int userId)
        {
            var existingOtp = await _otpRepository.GetOtpByUserIdAsync(userId);

            if (existingOtp != null)
            {
                await _otpRepository.DeleteOtpAsync(existingOtp);
            }

            string generatedOtp = OtpGenerator.GenerateOtp(6);

            // implement email logic

            Console.WriteLine("OTP: " + generatedOtp);

            var hashedOtp = _encryptionService.HashPassword(generatedOtp);

            var otp = new LoginOtp
            {
                UserId = userId,
                Otp = hashedOtp,
            };

            var result = await _otpRepository.StoreOtpAsync(otp);

            if (result == null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "OTP couldn't be sent.",
                };
            }

            return new AuthReturn
            {
                Status = true,
                Message = "OTP sent successfully."
            };
        }

        public async Task<AuthReturn> VerifyOtpAsync(int userId, string otp)
        {
            var userOtp = await _otpRepository.GetOtpByUserIdAsync(userId);

            if (userOtp == null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Session expired, please log in again."
                };
            }

            if (string.IsNullOrEmpty(userOtp.Otp))
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Invalid OTP entered."
                };
            }

            if (userOtp.CreatedAt.AddMinutes(10) < DateTime.UtcNow)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "OTP has expired, please request a new one."
                };
            }

            var otpMatches = _encryptionService.VerifyPassword(otp, userOtp.Otp);

            if (!otpMatches)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Invalid OTP entered."
                };
            }

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Session expired, please log in again."
                };
            }

            var authToken = _authTokenService.GenerateToken(user.Id, user.Email, user.Role);

            return new AuthReturn
            {
                Status = true,
                Message = "Successfully logged in.",
                AuthToken = authToken,
            };
        }
    }
}
