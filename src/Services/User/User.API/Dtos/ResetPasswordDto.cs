namespace User.API.Dtos
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Password {  get; set; }
        public string Token { get; set; }

    }
}
