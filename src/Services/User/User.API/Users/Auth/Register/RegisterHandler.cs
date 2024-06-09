
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

        }
    }

    public class RegisterCommandHandler
        (UserManager<ApplicationUser> _userManager,
        RoleManager<ApplicationRole> _roleManager)
        : ICommandHandler<RegisterCommand, RegisterResult>
    {
        public async  Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser appUser = new ApplicationUser
            {
                UserName = command.User.Name,
                Email = command.User.Email
            };

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
    }
}
