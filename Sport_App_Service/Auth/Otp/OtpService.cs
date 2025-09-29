using Hangfire;
using Sport_App_Model.Returns;
using Sport_App_Service.Encryption;
using Sports_App_Model.Entity;
using Sports_App_Repository.OtpRepository;
using Sports_App_Repository.UserRepository;
using Sports_App_Service.Email;
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
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobClient _backgroundJobs;

        public OtpService(IOtpRepository otpRepository, IEncryptionService encryptionService,
            IAuthTokenService authTokenService, IUserRepository userRepository, IEmailService emailService,
            IBackgroundJobClient backgroundJobs)
        {
            _otpRepository = otpRepository;
            _encryptionService = encryptionService;
            _authTokenService = authTokenService;
            _userRepository = userRepository;
            _emailService = emailService;
            _backgroundJobs = backgroundJobs;
        }

        public async Task<AuthReturn> StoreOtp(int userId)
        {
            var existingOtp = await _otpRepository.GetOtpByUserIdAsync(userId);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (existingOtp != null)
            {
                await _otpRepository.DeleteOtpAsync(existingOtp);
            }

            string generatedOtp = OtpGenerator.GenerateOtp(6);

            if (user == null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Session expired, please log in again."
                };
            }

            _backgroundJobs.Enqueue(() => _emailService.SendOtpEmailAsync(user.Email, generatedOtp));

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

        public async Task<AuthReturn> VerifyOtpAsync(string email, string otp)
        {
            var userOtp = await _otpRepository.GetOtpByUserEmailAsync(email);

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

            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Session expired, please log in again."
                };
            }

            await _otpRepository.DeleteOtpAsync(userOtp);
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
