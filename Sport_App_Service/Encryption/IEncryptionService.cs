namespace Sport_App_Service.Encryption
{
    public interface IEncryptionService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);    
    }
}
