namespace User.API.Dtos
{
    public class LoginResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string UserName { get; set; }=default!;
        public string ImageUrl { get; set; } = default!;
        public List<string> Roles { get; set; } 
        public string Token { get; set; }=default!;

    }
}
