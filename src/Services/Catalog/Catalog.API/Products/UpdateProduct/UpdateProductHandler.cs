
namespace Catalog.API.Products.UpdateProduct
{
    // UpdateProductQuery : đóng gói dữ liệu cần thiết cho yêu cầu đó
    // easy readable and maintenance
    public record UpdateProductCommand (Product Product) 
        :ICommand<UpdateProductResult>;
    public record UpdateProductResult (Product Product);
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
                throw new ProductNotFoundException();
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
