using System.Text.Json.Serialization;

namespace Sport_App_Model.Returns
{
    public class AuthReturn
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AuthToken { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string OtpToken { get; set; }
    }
}
