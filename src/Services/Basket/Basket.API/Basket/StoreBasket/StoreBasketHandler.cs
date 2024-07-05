
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart)
        : ICommand<StoreBasketResult>;
    public record StoreBasketResult (Guid UserId);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserId).NotEmpty().WithMessage("UserName is required");
        }
    }
    internal class StoreBasketCommandHandler (IBasketRepository repository
        , DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            // note it not check productId same
            await DeductDiscount(command.Cart, cancellationToken);
            // store basket in database (use Marten Upsert ) if exist = insert) and update Cache 
            var newBasket = await repository.StoreBasket(command.Cart, cancellationToken);
            return new StoreBasketResult(newBasket.UserId);

        }
        private async Task DeductDiscount (ShoppingCart cart, CancellationToken cancellationToken)
        {
            // communicate with Discount.Grpc and calculate lastest prices of products into shopping cart  
            foreach (var item in  cart.Items)
            {
                var coupon = await discountProtoService.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }
}
