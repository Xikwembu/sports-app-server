using Sport_App_Model.Returns;

namespace Sports_App_Service.Auth.Otp
{
    public interface IOtpService
    {
        Task<AuthReturn> StoreOtp(int userId);
        Task<AuthReturn> VerifyOtpAsync(string email, string otp);
    }
}
