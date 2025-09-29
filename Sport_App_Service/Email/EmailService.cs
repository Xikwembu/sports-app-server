using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Sports_App_Model.Setup;

namespace Sports_App_Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort = 587;
        private readonly string smtpUsername;
        private readonly string smtpPassword;
        private readonly string fromEmail;
        private readonly string fromName = "Sports Performance App";

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            var settings = emailSettings.Value;

            smtpServer = settings.MailServer;
            smtpUsername = settings.SenderEmail;
            smtpPassword = settings.SenderPassword;
            fromEmail = settings.SenderEmail;
        }

        public async Task SendOtpEmailAsync(string email, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Your Sports Performance App OTP";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""UTF-8"" />
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f7;
                            margin: 0;
                            padding: 0;
                            color: #333333;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 40px auto;
                            background: #ffffff;
                            border-radius: 8px;
                            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
                            padding: 30px;
                        }}
                        h2 {{
                            color: #2d89ef;
                            text-align: center;
                        }}
                        .otp {{
                            display: inline-block;
                            font-size: 28px;
                            letter-spacing: 4px;
                            font-weight: bold;
                            background: #f0f8ff;
                            border: 2px dashed #2d89ef;
                            padding: 12px 20px;
                            margin: 20px auto;
                            border-radius: 6px;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 13px;
                            color: #777;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <h2>Your One-Time Pin</h2>
                        <p>Hello,</p>
                        <p>Use the following code to complete your verification. This code will expire in <strong>10 minutes</strong>.</p>
                        <div class=""otp"">{otp}</div>
                        <p>If you didn't request this code, you can safely ignore this email.</p>
                    <div class=""footer"">
                        <p>Sports Performance App</p>
                    </div>
                    </div>
                </body>
            </html>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(smtpUsername, smtpPassword);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to send OTP email: " + ex.Message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
