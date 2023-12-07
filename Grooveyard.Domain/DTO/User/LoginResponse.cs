namespace Grooveyard.Domain.DTO.User
{
    public class LoginResponse
    {
        public TokenResponse Tokens { get; set; }
        public UserDTO User { get; set; }
    }
}
