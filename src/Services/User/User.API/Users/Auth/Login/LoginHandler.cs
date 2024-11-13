
using Basket.API.Exception;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.Net.WebSockets;
using User.API.Mapper;
using User.API.Models;
using User.API.Service;
using User.API.Users.Auth.Register;

namespace User.API.Users.Auth.Login
{
    public record LoginCommand (string Email, string Password)
        :ICommand<LoginResult>;
    public record LoginResult (UserDto User, string Token);

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

        }
    }

    public class LoginCommandHandler
        (UserManager<ApplicationUser> _userManager,
        IJwtTokenGenerator _jwtToken)
        : ICommandHandler<LoginCommand, LoginResult>
    {
        public async  Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(command.Email);
            if (appUser != null)
            {
                if (await _userManager.CheckPasswordAsync(appUser, command.Password))
                {
                    // generate token thôi ng? ?ã ha

                    var userDto = appUser.ToUserDto();
                   
                    string token = await _jwtToken.CreateJwtToken(appUser, Authorization.AccessTokenExpiredTimeInMinutes);
                    //// get role
                    //var roles = await _userManager.GetRolesAsync(appUser);
                    //loginResponseDto.Roles = roles.ToList();
                    return new LoginResult(userDto, token);
                }
            }
            throw new UserNotFoundException("Login Failed: Invalid Email or Password");
        }
    }
}
