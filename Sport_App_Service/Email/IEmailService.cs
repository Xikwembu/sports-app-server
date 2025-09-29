namespace Sports_App_Service.Email
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string email, string otp);
    }
}
