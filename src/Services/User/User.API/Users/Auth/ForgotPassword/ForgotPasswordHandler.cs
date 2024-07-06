namespace User.API.Users.Auth.ForgotPassword
{
    public record ForgotPasswordCommand(string Email)
        : ICommand<ForgotPasswordResult>;
    public record ForgotPasswordResult(bool IsSuccess);


    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

        }
    }
    public class ForgotPasswordCommandHandler
        (UserManager<ApplicationUser> _userManager,
        IJwtTokenGenerator _jwtToken,
        IEmailSender _emailSender)
        : ICommandHandler<ForgotPasswordCommand, ForgotPasswordResult>
    {
        public async Task<ForgotPasswordResult> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            // check email exist in system
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user is null)
                throw new UserNotFoundException(command.Email);
            // Generate the reset password token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await sendEmailResetPassword(command.Email, token);
            return new ForgotPasswordResult(true);

        }

        private async Task sendEmailResetPassword(string email, string token)
        {
            var message = $"[Valid within {Authorization.ResetPasswordExpiredTimeInMinutes} mins] Click the following link to reset your password: https://yourapp.com/reset-password?token={token}";
            _emailSender.SendEmailAsync(email, "Reset Password", message);
        }
    }
}
