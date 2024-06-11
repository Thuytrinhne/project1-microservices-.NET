using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace User.API.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string Name { get; set; } = default!;
        public UserImage UserImage { get; set; } = default!;
        public int Gender { get; set; } = default!;
        public DateTime DOB { get; set; } = default!;
        public List<string> RoleNames { get; set; } = default!;
        public List<UserAddress> UserAddresses { get; set; } = default!;
    }
    public class UserImage
    {
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
    }
}
