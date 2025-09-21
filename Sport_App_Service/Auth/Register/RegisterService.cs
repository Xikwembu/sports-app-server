using Sport_App_Model.Entity;
using Sport_App_Model.Returns;
using Sport_App_Service.Encryption;
using Sport_App_Service.Validation;
using Sports_App_Model.Dto;
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

        public async Task<AuthReturn> RegisterUserAsync(UserDto userDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);

            if (existingUser != null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Email already in use"
                };
            }

            var userByIdNumber = await _userRepository.GetUserByIdNumberAsync(userDto.IdNumber);
            if (userByIdNumber != null)
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "ID number already exists"
                };
            }

            if (!Validator.IsValidPassword(userDto.Password))
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "Password must be at least 8 characters, contain an uppercase, lowercase, digit, and special character"
                };
            }

            if (!Validator.IsValidIdNumber(userDto.IdNumber))
            {
                return new AuthReturn
                {
                    Status = false,
                    Message = "ID number must be 13 digits long and contain only numbers"
                };
            }

            var hashedPassword = _encryptionService.HashPassword(userDto.Password);

            var newUser = new User
            {
                Surname = userDto.Surname,
                Role = userDto.Role,
                RoleType = userDto.RoleType,
                Race = userDto.Race,
                Name = userDto.Name,
                IdNumber = userDto.IdNumber,
                Email = userDto.Email,
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
