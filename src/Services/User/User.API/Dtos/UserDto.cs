namespace User.API.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public List<string> RoleNames { get; set; } = default!;

        public int Gender { get; set; } = default!;
        public DateTime DOB { get; set; } = default!;
        public List<UserAddress> UserAddresses { get; set; } = default!;

    }
}
