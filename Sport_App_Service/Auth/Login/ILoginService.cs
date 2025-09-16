using Sport_App_Model.Returns;

namespace Sport_App_Service.Auth.Login
{
    public interface ILoginService
    {
        AuthReturn LoginReturn(string email, string password);
    }
}
