using Mapster;
using User.API.Mapper;
using User.API.Users.Auth.ResetPassword;

namespace User.API.Users.Account.UpdateAccount
{
    public record UpdateAccountCommand (Guid UserId, UpdateAccountDto UpdateAccountDto)
        : ICommand<UpdateAccountResult>;
    public record UpdateAccountResult (UserDto User);

    public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
              
        }
    }

    public class UpdateAccountCommandHandler
        (UserManager<ApplicationUser> _userManager )
        : ICommandHandler<UpdateAccountCommand, UpdateAccountResult>
    {
        public  async Task<UpdateAccountResult> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString());
            if  (user is  null)
            {
                throw new UserNotFoundException(command.UserId.ToString());
            }
            if (!string.IsNullOrEmpty(command.UpdateAccountDto.Name)){
                user.Name = command.UpdateAccountDto.Name;
            }
            if (command.UpdateAccountDto.Gender.HasValue && command.UpdateAccountDto.Gender != 0 )
            {
                user.Gender = command.UpdateAccountDto.Gender.Value;  
            }
            if (command.UpdateAccountDto.DOB.HasValue && command.UpdateAccountDto.DOB.Value != DateTime.MinValue)
            {
                user.DOB = command.UpdateAccountDto.DOB.Value;
            }
          
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new UpdateAccountResult(user.ToUserDto());
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
