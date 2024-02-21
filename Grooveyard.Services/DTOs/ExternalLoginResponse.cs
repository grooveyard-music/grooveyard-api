namespace Grooveyard.Services.DTOs
{
    public class ExternalLoginResponse
    {
        public TokenResponse Tokens { get; set; }
        public UserDTO User { get; set; }
        public bool IsNewUser { get; set; }
    }
}
