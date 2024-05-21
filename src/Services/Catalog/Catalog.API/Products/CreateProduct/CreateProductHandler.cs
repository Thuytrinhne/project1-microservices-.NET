namespace Catalog.API.Products.CreateProduct;

    public record CreateProductCommand (CreateProductDto Product)
        :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid ProductId);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand> 
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Product.Price).GreaterThanOrEqualTo(0).WithMessage("Price is greater than or Equal to 0");
        }
    }
    internal class CreateProductCommandHandler
        (IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create Product entity from command object
            // save to database
            // return CreateProductResult result
            var product = command.Product.Adapt<Product>();

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
      
            return new CreateProductResult(product.Id);
        }
    }

