using System.Text.RegularExpressions;

namespace Sport_App_Service.Validation
{
    public class Validator
    {
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
            return regex.IsMatch(password);
        }

        public static bool IsValidIdNumber(string idNumber)
        {
            if (string.IsNullOrWhiteSpace(idNumber))
            {
                return false;
            }

            var regex = new Regex(@"^\d{13}$");
            return regex.IsMatch(idNumber);
        }
    }
}
