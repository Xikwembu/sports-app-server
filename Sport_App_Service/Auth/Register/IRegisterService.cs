using Sport_App_Model.Returns;

namespace Sport_App_Service.Auth.Register
{
    public interface IRegisterService
    {
        Task<AuthReturn> RegisterUserAsync(string name, string surname, string email, string password, string role, string race, string idNumber, string roletype);
    }
}
