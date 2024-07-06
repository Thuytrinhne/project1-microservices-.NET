using FluentValidation;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(Guid OrderId, OrderDto Order)
     : ICommand<UpdateOrderResult>;
    public record UpdateOrderResult (bool IsSuccess);
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Id is required");
        }
    }

}
