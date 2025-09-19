using Microsoft.EntityFrameworkCore;
using Sport_App_Model;
using Sports_App_Model.Entity;

namespace Sports_App_Repository.OtpRepository
{
    public class OtpRepository : IOtpRepository
    {
        private readonly AppDbContext _dbContext;

        public OtpRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LoginOtp> StoreOtpAsync(LoginOtp otp)
        {
            _dbContext.LoginOtps.Add(otp);
            await _dbContext.SaveChangesAsync();

            return otp;
        }

        public async Task<LoginOtp?> GetOtpByUserIdAsync(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.LoginOtp)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                return user.LoginOtp;
            }

            return null;
        }

        public async Task<LoginOtp?> GetOtpByUserEmailAsync(string email)
        {
            var user = await _dbContext.Users
                .Include(u => u.LoginOtp)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                return user.LoginOtp;
            }

            return null;
        }

        public async Task DeleteOtpAsync(LoginOtp otp)
        {
            _dbContext.LoginOtps.Remove(otp);
            await _dbContext.SaveChangesAsync();
        }
    }
}
