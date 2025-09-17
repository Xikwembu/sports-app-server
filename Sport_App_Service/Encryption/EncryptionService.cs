namespace Sport_App_Service.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        public string HashPassword(string password)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(password);

            return hashed;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            return verified;
        }
    }
}
