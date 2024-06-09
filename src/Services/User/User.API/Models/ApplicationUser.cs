using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace User.API.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string ImageUrl { get; set; } = default!;
        public string PublicId { get; set; } = default!;
    }
}
