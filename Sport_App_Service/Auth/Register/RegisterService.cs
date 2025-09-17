using Sport_App_Model.Entity;
using Sport_App_Model.Returns;
using Sport_App_Service.Encryption;
using Sports_App_Repository.UserRepository;

namespace Sport_App_Service.Auth.Register
{
    public class RegisterService : IRegisterService
    {
        public readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public RegisterService(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public AuthReturn RegisterUser(string name, string surname, string email, string password, string role, string race, string idNumber)
        {
            var existingUser = _userRepository.GetUserByEmailAsync(email).Result;

            if (existingUser != null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Messsage = "Email already in use"
                };
            }

            var hashedPassword = _encryptionService.HashPassword(password);

            var newUser = new User
            {
                Name = name,
                Surname = surname,
                Email = email,
                Password = hashedPassword,
                Role = role,
                Race = race,
                IdNumber = idNumber
            };

            _userRepository.AddUserAsync(newUser).Wait();

            return new AuthReturn
            {
                Status = true,
                Messsage = "User registered successfully"
            };
        }
    }
}
