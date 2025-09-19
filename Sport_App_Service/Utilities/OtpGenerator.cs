﻿namespace Sports_App_Service.Utilities
{
    public class OtpGenerator
    {
        public static string GenerateOtp(int length = 6)
        {
            var random = new Random();
            var otp = "";

            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10);
            }

            return otp;
        }
    }
}
