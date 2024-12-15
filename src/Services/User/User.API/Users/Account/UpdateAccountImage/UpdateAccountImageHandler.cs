using BuildingBlocks.CQRS;
using BuildingBlocks.PhotoCloudinary;
using User.API.Mapper;
using User.API.Users.Account.UpdateAccount;

namespace Catalog.API.Products
{
    public record UpdateAccountImageCommand(Guid UserId, IFormFile Image):ICommand<UpdateAccountImageResult>;
    public record UpdateAccountImageResult(bool IsSuccess);

    public class UpdateAccountImageValidator : AbstractValidator<UpdateAccountImageCommand>
    {
        public UpdateAccountImageValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("User Id is required");
            RuleFor(x => x.Image).NotEmpty().NotNull().WithMessage("Image is required");

        }
    }
    public class UpdateAccountImageHandler
        (ICloudinaryService _cloudinaryService, UserManager<ApplicationUser> _userManager )
        : ICommandHandler<UpdateAccountImageCommand, UpdateAccountImageResult>
    {
        public async  Task<UpdateAccountImageResult> Handle(UpdateAccountImageCommand command, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByIdAsync(command.UserId.ToString());
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId.ToString());
            }

            var resultFromCloud = await _cloudinaryService.AddPhotoAsync(command.Image);
                if (resultFromCloud.Error is not null)
                {
                    throw new Exception("Image is not valid to update");
                }
                if (user.UserImage is not null)
                    await _cloudinaryService.DeletePhotoAsync(user.UserImage.PublicId);

                user.UserImage = new UserImage
                {
                    ImageUrl = resultFromCloud.SecureUri.AbsoluteUri,
                    PublicId = resultFromCloud.PublicId
                };
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new UpdateAccountImageResult(true);
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
