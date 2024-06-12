using BuildingBlocks.PhotoCloudinary;
using CloudinaryDotNet;

namespace Catalog.API.Products.CreateProduct;

    public record CreateProductCommand(
    string Name,
    string Title,
    Guid CategoryId,
    string Description,
    IFormFile Image,
    decimal Price,
     string Tags)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid ProductId);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand> 
    {
        public CreateProductCommandValidator()
        {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price is greater than or Equal to 0");
        }
    }
    internal class CreateProductCommandHandler
        (IDocumentSession session, ICloudinaryService _cloudinary)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create Product entity from command object
            // save to database
            // return CreateProductResult result
            var product = command.Adapt<Product>();
            var category  = await session.LoadAsync<Category>(command.CategoryId, cancellationToken);
            //if (category is null)
            //{
            //    throw new CategoryNotFoundException(command.CategoryId);
            //}    
            product.Category = category;

            if(command.Image is not null)
            {
                var resutl = await _cloudinary.AddPhotoAsync(command.Image);
                if (resutl.Error is not null)
                {
                    throw new Exception("Image is not valid to update");
                }
                product.Image = new CatalogImage
                {
                    ImageUrl = resutl.SecureUri.AbsoluteUri,
                    PublicId = resutl.PublicId
                };
             }    
            
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
      
            return new CreateProductResult(product.Id);
        }
    }

