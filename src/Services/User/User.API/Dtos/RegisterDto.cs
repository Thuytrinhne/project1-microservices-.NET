namespace User.API.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string OTP { get; set; } = default!;

        public int Gender { get; set; } = default!;

        public DateTime DOB { get; set; } = default!;


    }
}
