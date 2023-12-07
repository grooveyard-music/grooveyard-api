namespace Grooveyard.Domain.DTO.User
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; }
    }

}
