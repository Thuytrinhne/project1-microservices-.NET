
using Microsoft.AspNetCore.Identity;
using User.API.Users.Auth.ForgotPassword;

namespace User.API.Users.Auth.ResetPassword
{
    public record ResetPasswordCommand (ResetPasswordDto ResetPasswordDto)
        :ICommand<ResetPasswordResult>;
    public record ResetPasswordResult(bool IsSuccess);

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.ResetPasswordDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.ResetPasswordDto.Password)
               .NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ResetPasswordDto.Token)
             .NotEmpty().WithMessage("Token is required");
        }
    }

    public class ResetPasswordCommandHandler
        (UserManager<ApplicationUser> _userManager)
        : ICommandHandler<ResetPasswordCommand, ResetPasswordResult>
    {
        public async   Task<ResetPasswordResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(command.ResetPasswordDto.Email);

            if (user is not  null)
            {
                // reset the user password
                var result = await _userManager.ResetPasswordAsync(user, command.ResetPasswordDto.Token, command.ResetPasswordDto.Password);

                if (result.Succeeded)
                {
                    return new   ResetPasswordResult(true);
                }

                
                List<string> errorMessages = new List<string>();
                foreach (var error in result.Errors)
                {
                    errorMessages.Add(error.Description);

                }
                // Throw a single exception with all error messages
                throw new Exception(string.Join("; ", errorMessages));
            }
            throw new  UserNotFoundException(command.ResetPasswordDto.Email);

        }
    }
}
