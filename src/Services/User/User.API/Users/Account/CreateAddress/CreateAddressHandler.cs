using Mapster;
using MongoDB.Driver;
using User.API.Users.Account.UpdateAccount;

namespace User.API.Users.Account.CreateAddress
{
    public record CreateAddressCommand (Guid UserId,
        string Phone,string Name,
     int Default,
     string Province,
     string District,
     string Ward,
     string DetailAddress)
        :ICommand<CreateAddressResult>;
    public record CreateAddressResult (bool IsSuccess);

    public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required");

            RuleFor(x => x.Default)
            .NotEmpty().WithMessage("Default is required");

            RuleFor(x => x.Province)
            .NotEmpty().WithMessage("Province is required");

            RuleFor(x => x.District)
            .NotEmpty().WithMessage("District is required");

            RuleFor(x => x.Ward)
            .NotEmpty().WithMessage("Ward is required");

            RuleFor(x => x.DetailAddress)
            .NotEmpty().WithMessage("DetailAddress is required");

        }
    }
    public class CreateAddressCommandHandler
        (UserManager<ApplicationUser> _userManager)
        : ICommandHandler<CreateAddressCommand, CreateAddressResult>
    {
        public async  Task<CreateAddressResult> Handle(CreateAddressCommand command, CancellationToken cancellationToken)
        {
           var user =  await _userManager.FindByIdAsync(command.UserId.ToString());
            var newAddress = command.Adapt<UserAddress>();
            if (user is not null)
            {
                if (user.UserAddresses is null)
                {
                    user.UserAddresses = new List<UserAddress>();
                    newAddress.Default = 1;
                }
                else
                {
                    if(command.Default == 1)
                    {
                       var UserDefaultAddress =  user.UserAddresses.FirstOrDefault(addr => addr.Default == 1);
                       UserDefaultAddress.Default = 0; 
                    }

                }

                user.UserAddresses.Add(newAddress);
                var result =  await _userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return new CreateAddressResult(true);
                }

                List<string> errorMessages = new List<string>();
                foreach (IdentityError error in result.Errors)
                {
                    errorMessages.Add(error.Description);
                }

                // Throw a single exception with all error messages
                throw new Exception(string.Join("; ", errorMessages));

            }
            throw new UserNotFoundException(command.UserId.ToString());
        }
    }

}
