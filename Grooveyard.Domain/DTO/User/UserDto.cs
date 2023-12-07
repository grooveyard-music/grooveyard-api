namespace Grooveyard.Domain.DTO.User
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
        public string Avatar { get; set; }
    }
}
