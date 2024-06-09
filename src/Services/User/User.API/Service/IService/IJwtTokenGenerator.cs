namespace User.API.Service.IService
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateJwtToken(ApplicationUser user);
   }
}
