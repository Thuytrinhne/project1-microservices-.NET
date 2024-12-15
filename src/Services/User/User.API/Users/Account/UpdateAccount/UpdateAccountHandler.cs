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
        (UserManager<ApplicationUser> _userManager,
        ICloudinaryService _cloudinaryService)
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
            if (command.UpdateAccountDto.Gender.HasValue)
            {
                user.Gender = command.UpdateAccountDto.Gender.Value;  
            }
            if (command.UpdateAccountDto.DOB.HasValue)
            {
                user.DOB = command.UpdateAccountDto.DOB.Value;
            }
            if (command.UpdateAccountDto.Image != null)
            {
                var resultFromCloud =  await _cloudinaryService.AddPhotoAsync(command.UpdateAccountDto.Image);
               if ( resultFromCloud.Error is not null )
               {
                    throw new Exception("Image is not valid to update");
               }
                if (user.UserImage is not null)
                   await  _cloudinaryService.DeletePhotoAsync(user.UserImage.PublicId);

                user.UserImage = new UserImage {ImageUrl = resultFromCloud.SecureUri.AbsoluteUri ,
                    PublicId = resultFromCloud.PublicId}; 
               
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
