namespace Grooveyard.Services.DTOs
{
    public class LoginResponse
    {
        public TokenResponse Tokens { get; set; }
        public UserDTO User { get; set; }
    }
}
