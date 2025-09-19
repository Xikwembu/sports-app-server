using Sports_App_Model.Entity;

namespace Sports_App_Repository.OtpRepository
{
    public interface IOtpRepository
    {
        Task<LoginOtp> StoreOtpAsync(LoginOtp otp);
        Task<LoginOtp?> GetOtpByUserIdAsync(int userId);
        Task<LoginOtp?> GetOtpByUserEmailAsync(string email);
        Task DeleteOtpAsync(LoginOtp otp);
    }
}
