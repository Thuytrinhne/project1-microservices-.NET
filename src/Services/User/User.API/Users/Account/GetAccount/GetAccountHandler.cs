using Mapster;
using MongoDB.Driver;
using User.API.Mapper;
using User.API.Users.Account.DeleteAddress;

namespace User.API.Users.Account.GetAccount
{
    public class GetAccountHandler
    {
        public record GetAccountQuery(Guid UserId)
        : ICommand<GetAccountResult>;
        public record GetAccountResult(UserDto User);
        public class GetAccountValidator : AbstractValidator<GetAccountQuery>
        {
            public GetAccountValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId is required");  
            }
        }
        public class GetAccountQueryHandler
            (UserManager<ApplicationUser> _userManager)
            : ICommandHandler<GetAccountQuery, GetAccountResult>
        {
            public async  Task<GetAccountResult> Handle(GetAccountQuery query, CancellationToken cancellationToken)
            {
                var appUser = await _userManager.FindByIdAsync(query.UserId.ToString());
                if (appUser != null)
                { 
                    var userDto = appUser.ToUserDto();
                    return new GetAccountResult(userDto);
                }
                
                throw new UserNotFoundException(query.UserId.ToString());

            }
        }
    } 

        
}
