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

        public UserImage UserImage { get; set; }
        public int Gender { get; set; } = default!;

        public DateTime DOB { get; set; } = default!;
        public List<string> RoleNames { get; set; } = default!;
        public List<UserAddress> UserAddresses { get; set; } = default!;

    }
  
}
