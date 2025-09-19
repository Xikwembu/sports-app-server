using Sport_App_Model.Entity;
using Sport_App_Model.Returns;
using Sport_App_Service.Encryption;
using Sport_App_Service.Validation;
using Sports_App_Repository.UserRepository;

namespace Sport_App_Service.Auth.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public RegisterService(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public async Task<AuthReturn> RegisterUserAsync(string name, string surname, string email, string password, string role, string race, string idNumber, string roleType)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Email already in use"
                };
            }

            var userByIdNumber = await _userRepository.GetUserByIdNumberAsync(idNumber);
            if (userByIdNumber != null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "ID number already exists"
                };
            }

            if (!Validator.IsValidPassword(password))
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Password must be at least 8 characters, contain an uppercase, lowercase, digit, and special character"
                };
            }

            if (!Validator.IsValidIdNumber(idNumber))
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "ID number must be 13 digits long and contain only numbers"
                };
            }

            var hashedPassword = _encryptionService.HashPassword(password);

            var newUser = new User
            {
                Surname = surname,
                Role = role,
                RoleType = roleType,
                Race = race,
                Name = name,
                IdNumber = idNumber,
                Email = email,
                Password = hashedPassword,
            };

            await _userRepository.AddUserAsync(newUser);

            return new AuthReturn
            {
                Status = true,
                Message = "User registered successfully"
            };
        }
    }
}
