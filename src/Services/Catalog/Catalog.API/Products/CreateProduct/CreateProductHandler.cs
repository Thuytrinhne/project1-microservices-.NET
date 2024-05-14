namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand (CreateProductDto Product)
        :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid ProductId);
    internal class CreateProductCommandHandler (IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create Product entity from command object
            // save to database
            // return CreateProductResult result
            var product = new Product
            {
                Name = command.Product.Name,
                Category = command.Product.Category,
                Description = command.Product.Description,
                ImageFile = command.Product.ImageFile,
                Price = command.Product.Price
            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
      
            return new CreateProductResult(product.Id);
        }
    }
}
