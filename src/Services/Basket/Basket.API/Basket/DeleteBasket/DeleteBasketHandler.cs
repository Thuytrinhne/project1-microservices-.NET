
namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand (Guid userId)
        : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult (bool IsSuccess);
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.userId).NotEmpty().WithMessage("UserName is required");

        }
    }
    internal class DeleteBasketCommandHandler (IBasketRepository repository)
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async  Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            var result   = await  repository.DeleteBasket(command.userId, cancellationToken);
            return new DeleteBasketResult(result);
        }
    }
}
