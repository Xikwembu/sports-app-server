using Sport_App_Model.Returns;
using Sports_App_Model.Dto;

namespace Sport_App_Service.Auth.Register
{
    public interface IRegisterService
    {
        Task<AuthReturn> RegisterUserAsync(UserDto userDto);
    }
}
