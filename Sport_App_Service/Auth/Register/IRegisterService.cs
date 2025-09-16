using Sport_App_Model.Returns;

namespace Sport_App_Service.Auth.Register
{
    public interface IRegisterService
    {
        AuthReturn RegisterReturn(string email, string password, string role);
    }
}
