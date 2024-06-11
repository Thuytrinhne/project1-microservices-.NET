using Mapster;

namespace User.API.Mapper
{
    public static class ApplicationUserExtension
    {
        public static UserDto ToUserDto(this ApplicationUser user)
        {
            var UserDto = user.Adapt<UserDto>();
            if (user.UserImage is not null)
            UserDto.ImageUrl = user.UserImage.ImageUrl;
            return UserDto;
        }
    }
}
