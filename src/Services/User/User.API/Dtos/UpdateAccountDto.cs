namespace User.API.Dtos
{
    public class UpdateAccountDto
    {
        public string Name { get; set; } = default!;
        public IFormFile Image { get; set; } = default!;
        public int Gender { get; set; } = default!;
        public DateTime DOB { get; set; } = default!;
    }
}
