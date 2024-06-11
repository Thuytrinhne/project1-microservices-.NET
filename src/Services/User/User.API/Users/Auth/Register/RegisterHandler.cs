
using Basket.API.Exception;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace User.API.Users.Auth.Register
{
    public record RegisterCommand (RegisterDto User)
        : ICommand<RegisterResult>;
    public record RegisterResult (bool IsSuccess);

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.User.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.User.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");

            RuleFor(x => x.User.Password).NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.User.OTP)
                .NotEmpty().WithMessage("OTP is required")
                .Length(6).WithMessage("OTP must be exactly 6 characters long");

        }
    }

    public class RegisterCommandHandler
        (UserManager<ApplicationUser> _userManager,
        RoleManager<ApplicationRole> _roleManager,
        EmailVerificationService _emailVerification)
        : ICommandHandler<RegisterCommand, RegisterResult>
    {
        public async  Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            // check OTP
            if (await  checValidOTP(command.User.Email, command.User.OTP)==false)
                return new RegisterResult(false);



            var appUser = command.User.Adapt<ApplicationUser>();
            appUser.UserName = command.User.Email;
            appUser.RoleNames = new List<string> { Authorization.DefaultRole.ToString() };

            IdentityResult result = await _userManager.CreateAsync(appUser, command.User.Password);
            if (result.Succeeded)
            {
                string defaultRole = Authorization.DefaultRole.ToString();
                if (!await _roleManager.RoleExistsAsync(defaultRole))
                {
                    IdentityResult resultAddRole = await _roleManager.CreateAsync(new ApplicationRole() { Name = Authorization.DefaultRole.ToString() });
                    if (!resultAddRole.Succeeded)
                    {
                        throw new Exception(resultAddRole.Errors.ToString());
                    }    
                    
                }

                await _userManager.AddToRoleAsync(appUser, Authorization.DefaultRole.ToString());
               
                return new RegisterResult(true);
            }
            else
            {
                List<string> errorMessages = new List<string>();
                foreach (IdentityError error in result.Errors)
                {
                    errorMessages.Add(error.Description);
                }

                // Throw a single exception with all error messages
                throw new Exception(string.Join("; ", errorMessages));
            }
        }

        private async  Task<bool>  checValidOTP(string email, string otp)
        {
            var obj= await _emailVerification.GetByEmailAsync(email);
            if (obj is null)
                return false;
            if(obj.ExpiryTime >= DateTime.UtcNow && otp == obj.OTP) return true;
            return false;

        }
    }
}
