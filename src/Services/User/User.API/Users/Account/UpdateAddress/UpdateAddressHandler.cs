using Mapster;
using User.API.Users.Account.UpdateAccount;

namespace User.API.Users.Account.UpdateAddress
{
    public record UpdateAddressCommand (Guid UserId, UpdateUserAddressDto UserAddress)
        :ICommand<UpdateAddressResult>;
    public record UpdateAddressResult(UserAddressDto UserAddress);
    public class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.UserAddress.Id)
               .NotEmpty().WithMessage("UserAddressId is required");
        }
    }
    public class UpdateAddressCommandHandler
        (UserManager <ApplicationUser> _userManager)
        : ICommandHandler<UpdateAddressCommand, UpdateAddressResult>
    {
        public async Task<UpdateAddressResult> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString());
            if (user is null) throw new UserNotFoundException(command.UserId.ToString());

            var userAddress = user.UserAddresses.Find(x => x.Id == command.UserAddress.Id);
            if (userAddress is null)
            {
                throw new Exception($"UserAddress {command.UserAddress.Id} not found");
            }

            if(!string.IsNullOrEmpty(command.UserAddress.Name))
            {
                userAddress.Name = command.UserAddress.Name;
            }
            if (!string.IsNullOrEmpty(command.UserAddress.Phone))
            {
                userAddress.Phone = command.UserAddress.Phone;

            }
            if (!string.IsNullOrEmpty(command.UserAddress.Province))
            {
                userAddress.Province = command.UserAddress.Province;

            }
            if (!string.IsNullOrEmpty(command.UserAddress.District))
            {
                userAddress.District = command.UserAddress.District;

            }
            if (!string.IsNullOrEmpty(command.UserAddress.Ward))
            {
                userAddress.Ward = command.UserAddress.Ward;

            }
            if (!string.IsNullOrEmpty(command.UserAddress.DetailAddress))
            {
                userAddress.DetailAddress = command.UserAddress.DetailAddress;

            }
            if (command.UserAddress.Default is not  null )
            {
                if(command.UserAddress.Default == 1)
                {
                    var UserDefaultAddress = user.UserAddresses.FirstOrDefault(addr => addr.Default == 1);
                    UserDefaultAddress.Default = 0;
                    userAddress.Default = 1;

                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) {
                return new UpdateAddressResult(userAddress.Adapt<UserAddressDto>());
            }

            List<string> errorMessages = new List<string>();
            foreach (IdentityError error in result.Errors)
            {
                errorMessages.Add(error.Description);
            }

            // Throw a single exception with all error messages
            throw new Exception(string.Join("; ", errorMessages));
        }
    }

}
