
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
    public record LoginCommand (LoginDto User)
        :ICommand<LoginResult>;
    public record LoginResult (LoginResponseDto User);

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.User.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.User.Password).NotEmpty().WithMessage("Password is required");

        }
    }

    public class LoginCommandHandler
        (UserManager<ApplicationUser> _userManager,
        IJwtTokenGenerator _jwtToken)
        : ICommandHandler<LoginCommand, LoginResult>
    {
        public async  Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(command.User.Email);
            if (appUser != null)
            {
                if (await _userManager.CheckPasswordAsync(appUser, command.User.Password))
                {
                    // generate token
                    var userDto = appUser.ToUserDto();
                    LoginResponseDto loginResponse = new LoginResponseDto();
                    loginResponse.User = userDto;
                    loginResponse.Token = await _jwtToken.CreateJwtToken(appUser, Authorization.AccessTokenExpiredTimeInMinutes);
                    //// get role
                    //var roles = await _userManager.GetRolesAsync(appUser);
                    //loginResponseDto.Roles = roles.ToList();
                    return new LoginResult(loginResponse);
                }
            }
            throw new UserNotFoundException("Login Failed: Invalid Email or Password");
        }
    }
}
