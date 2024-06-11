
using User.API.Models;
using User.API.Users.Account.CreateAddress;

namespace User.API.Users.Account.DeleteAddress
{
    public record DeleteAddressCommand (Guid UserId, Guid AddressId)
        :ICommand<DeleteAddressResult>;

    public record DeleteAddressResult(bool IsSuccess);

    public class DeleteAddressValidator : AbstractValidator<DeleteAddressCommand>
    {
        public DeleteAddressValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.AddressId)
              .NotEmpty().WithMessage("AddressId is required");
        }
    }
    public class DeleteAddressCommandHandler
        (UserManager<ApplicationUser> _userManager)
        :ICommandHandler<DeleteAddressCommand, DeleteAddressResult>
    {
        public async Task<DeleteAddressResult> Handle(DeleteAddressCommand command, CancellationToken cancellationToken)
        {
            var user = await  _userManager.FindByIdAsync(command.UserId.ToString());
            if(user is  null)
            {
                throw  new UserNotFoundException(command.UserId.ToString());
            }

            var userAddress =  user.UserAddresses.Find(x=>x.Id == command.AddressId);
            if(userAddress is null)
            {
                throw  new NotFoundException($"UserAddress {command.AddressId} is not valid.");
            }
            if(userAddress.Default == 1)
                throw new Exception($"Cannot delete the default address.");

            user.UserAddresses.Remove(userAddress);
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return new DeleteAddressResult(true);
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
