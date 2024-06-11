using BuildingBlocks.CQRS;
using BuildingBlocks.PhotoCloudinary;
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products
{
    public record  UpdateProductImageCommand (Guid ProducId, IFormFile Image):ICommand<UpdateProductImageResult>;
    public record UpdateProductImageResult (bool IsSuccess);

    public class UpdateProductImageValidator : AbstractValidator<UpdateProductImageCommand>
    {
        public UpdateProductImageValidator()
        {
            RuleFor(x => x.ProducId).NotEmpty().NotNull().WithMessage("Product Id is required");
            RuleFor(x => x.Image).NotEmpty().NotNull().WithMessage("Product Id is required");

        }
    }
    public class UpdateProductImageHandler
        (ICloudinaryService _cloudinaryService, IDocumentSession _documentSession )
        : ICommandHandler<UpdateProductImageCommand, UpdateProductImageResult>
    {
        public async  Task<UpdateProductImageResult> Handle(UpdateProductImageCommand command, CancellationToken cancellationToken)
        {

            var productFromDb = await _documentSession.Query<Product>()
               .Where(p => p.Id == command.ProducId)
               .FirstAsync(cancellationToken);
            if (productFromDb is null)
                throw new ProductNotFoundException(command.ProducId);
            
                var resultFromCloud = await _cloudinaryService.AddPhotoAsync(command.Image);
                if (resultFromCloud.Error is not null)
                {
                    throw new Exception("Image is not valid to update");
                }
                if (productFromDb.Image is not null)
                    await _cloudinaryService.DeletePhotoAsync(productFromDb.Image.PublicId);

                productFromDb.Image = new CatalogImage
                {
                    ImageUrl = resultFromCloud.SecureUri.AbsoluteUri,
                    PublicId = resultFromCloud.PublicId
                };
                _documentSession.Update(productFromDb);
                await _documentSession.SaveChangesAsync();

            return new UpdateProductImageResult(true);

        }
    }

}
