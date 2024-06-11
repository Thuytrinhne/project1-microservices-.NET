namespace User.API.Dtos
{
    public class CreateUserAddressDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Default { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string DetailAddress { get; set; }
    }
}
