using Sport_App_Model.Entity;

namespace Sports_App_Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByIdNumberAsync(string idNumber);
    }
}
