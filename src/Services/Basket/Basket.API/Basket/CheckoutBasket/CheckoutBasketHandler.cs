
using Basket.API.Basket.StoreBasket;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{

    public record CheckoutBasketCommand (BasketCheckoutDto BasketCheckoutDto)
        :ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult (bool IsSuccess);


    public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can not be null");
            RuleFor(x => x.BasketCheckoutDto.CustomerId).NotEmpty().WithMessage("UserName is required");
        }
    }
    internal class CheckoutBasketHandler
        (IBasketRepository repository, IPublishEndpoint publishEndpoint)
        : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            // get existing basket with total price
            // set totalprice on basket checkout event message
            // send basket checkout event to rabitmq using masstransit
            // delete the basket 
            var basket = await repository.GetBasket(command.BasketCheckoutDto.CustomerId, cancellationToken);
            if(basket == null)
            {
                return new CheckoutBasketResult(false);
            }
            var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;
            foreach (var item in basket.Items)
            {
                eventMessage.OrderItems.Add(new OrderItem(item.ProductId, item.Quantity, item.Price));
            }

            await publishEndpoint.Publish(eventMessage, cancellationToken);


            await repository.DeleteBasket(command.BasketCheckoutDto.CustomerId, cancellationToken);


            return new CheckoutBasketResult(true);
        }
    }
}
