
using BuildingBlocks.PhotoCloudinary;
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct
{
    // UpdateProductQuery : đóng gói dữ liệu cần thiết cho yêu cầu đó
    // easy readable and maintenance
    public record UpdateProductCommand(
     Guid Id, 
     string ? Title,
    Guid ? CategoryId,
    string ? Description,
    decimal ? Price,
    string ? Tags) 
        :ICommand<UpdateProductResult>;
    public record UpdateProductResult (ProductDto Product);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");

       
        }
    }
    internal class UpdateProducCommandHandler(IDocumentSession session, ICloudinaryService _cloudinaryService)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
      
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            // load sản phẩm đó ra
            // thay doi san pham
            // update vao db
           
            var productFromDb = await  session.LoadAsync<Product>(command.Id, cancellationToken);
            if (productFromDb is null)
            {
                throw new ProductNotFoundException(command.Id);
            }
            if (command.CategoryId != Guid.Empty)
            {
                var category = await session.LoadAsync<Category>(command.CategoryId.Value, cancellationToken);

                if (category is null) throw new CategoryNotFoundException(command.CategoryId.Value);
                productFromDb.Category = category;
            }
            if (!string.IsNullOrEmpty(  command.Title))
                productFromDb.Title = command.Title;
       
            if (!string.IsNullOrEmpty(command.Description))
            {
                productFromDb.Description = command.Description;

            }
            if (!string.IsNullOrEmpty(command.Tags))
            {
                productFromDb.Description = command.Tags;

            }
            if (command.Price >=0)
            {
                productFromDb.Price = command.Price.Value;

            }
            
            session.Update(productFromDb);
            await session.SaveChangesAsync();
            return new UpdateProductResult (productFromDb.Adapt<ProductDto>());
        }
    }
}
