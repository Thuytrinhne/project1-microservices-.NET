namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand (Product Product)
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
           
            session.Store(command.Product);
            await session.SaveChangesAsync(cancellationToken);
      
            return new CreateProductResult(command.Product.Id);
        }
    }
}
