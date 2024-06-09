using System.ComponentModel.DataAnnotations;

namespace User.API.Models
{
    public class User
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ImageUrl { get; set; } = default!;
        public string PublicId { get; set; } = default!;

    }
}
