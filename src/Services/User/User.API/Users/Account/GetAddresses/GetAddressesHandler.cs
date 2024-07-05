using Mapster;
using MongoDB.Driver;
using User.API.Users.Account.DeleteAddress;

namespace User.API.Users.Account.GetAddresses
{
    public class GetAddressesHandler
    {
        public record GetAddressesQuery(Guid UserId, Guid AddressId, int defaultAddress)
        : ICommand<GetAddressesResult>;
        public record GetAddressesResult(IEnumerable<UserAddressDto> ListAddress);
        public class GetAddressesValidator : AbstractValidator<GetAddressesQuery>
        {
            public GetAddressesValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId is required");  
            }
        }
        public class GetAddressesQueryHandler
            (UserManager<ApplicationUser> _userManager)
            : ICommandHandler<GetAddressesQuery, GetAddressesResult>
        {
            public async  Task<GetAddressesResult> Handle(GetAddressesQuery query, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(query.UserId.ToString());
                IEnumerable<UserAddress> userAddress;   
                if (user is null)
                {
                    throw new UserNotFoundException(query.UserId.ToString());
                }
                if (query.AddressId != Guid.Empty)
                {
                    var address = user.UserAddresses.Find(x => x.Id == query.AddressId);
                    userAddress = address != null ? new List<UserAddress> { address } : new List<UserAddress>();
                }
                else
                {
                    userAddress = user.UserAddresses;
                }
                if (query.defaultAddress != -1) {
                    userAddress = userAddress.Where(x => x.Default == query.defaultAddress).ToList();
                }
                return new GetAddressesResult(userAddress.Adapt<List<UserAddressDto>>());
            }
        }
    } 

        
}
