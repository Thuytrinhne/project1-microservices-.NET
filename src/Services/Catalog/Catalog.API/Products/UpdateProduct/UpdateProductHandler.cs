
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct
{
    // UpdateProductQuery : đóng gói dữ liệu cần thiết cho yêu cầu đó
    // easy readable and maintenance
    public record UpdateProductCommand (UpdateProductDto Product) 
        :ICommand<UpdateProductResult>;
    public record UpdateProductResult (Product Product);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Product Id is required");

            RuleFor(x => x.Product.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

            RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category is required");

            RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");

            RuleFor(x => x.Product.Price).GreaterThanOrEqualTo(0).WithMessage("Price is greater than or Equal to 0");
        }
    }
    internal class UpdateProducCommandHandler(IDocumentSession session, ILogger<GetProductsHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
      
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProducCommandHandler.Handle called with {@command}", command);
            // load sản phẩm đó ra
            // thay doi san pham
            // update vao db
           
            var productFromDb = await  session.LoadAsync<Product>(command.Product.Id, cancellationToken);
            if (productFromDb is null)
            {
                throw new ProductNotFoundException(command.Product.Id);
            }
            productFromDb.Name = command.Product.Name;
            productFromDb.ImageFile = command.Product.ImageFile;
            productFromDb.Category = command.Product.Category;
            productFromDb.Description = command.Product.Description;
            productFromDb.Price = command.Product.Price;
            
            session.Update(productFromDb);
            await session.SaveChangesAsync();
            return new UpdateProductResult (productFromDb);
        }
    }
}
