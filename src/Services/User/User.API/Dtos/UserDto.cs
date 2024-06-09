namespace User.API.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public List<string> Roles { get; set; }
    }
}
