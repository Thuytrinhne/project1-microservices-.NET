
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand (Guid ProductId)
        :ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required");
        }
    }
    internal class DeleteProductCommandHandler (ILogger<DeleteProductCommandHandler> logger, IDocumentSession session)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>

    {
        public async  Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler.Handle called with {@command}", command);
            var productFrmDB = await session.LoadAsync<Product>(command.ProductId);
            if(productFrmDB is null)
            {
                throw new ProductNotFoundException(command.ProductId);
            }
            else
            {
                 session.Delete<Product>(productFrmDB);
                 await session.SaveChangesAsync(cancellationToken);
                 return new DeleteProductResult(true);
            }    
        }
    }
}
