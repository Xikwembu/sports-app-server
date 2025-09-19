using Microsoft.EntityFrameworkCore;
using Sport_App_Model;
using Sport_App_Model.Entity;

namespace Sports_App_Repository.UserRepository
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                return user;
            }

            return null;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                return user;
            }

            return null;
        }

        public async Task<User?> GetUserByIdNumberAsync(string idNumber)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.IdNumber == idNumber);

            if (user != null)
            {
                return user;
            }

            return null;
        }
    }
}
