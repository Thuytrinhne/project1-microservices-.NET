namespace User.API.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
