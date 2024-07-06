using Ordering.Application.Exceptions;

namespace Ordering.Application.Orders.Commands.UpdateOrder;
public class UpdateOrderHandler(IAppDbContext dbContext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        //Update Order entity from command object
        //save to database
        //return result

        var orderId = OrderId.Of(command.OrderId);
        var order = await dbContext.Orders
            .FindAsync([orderId], cancellationToken: cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.Order.Id);
        }
       
        UpdateOrderWithNewValues(order, command.Order);

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    public void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        //var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.CustomerName, orderDto.ShippingAddress.Phone, orderDto.ShippingAddress.Province, orderDto.ShippingAddress.District, orderDto.ShippingAddress.Ward, orderDto.ShippingAddress.DetailAddress);
        //var updatedBillingAddress = Address.Of(orderDto.ShippingAddress.CustomerName, orderDto.ShippingAddress.Phone, orderDto.ShippingAddress.Province, orderDto.ShippingAddress.District, orderDto.ShippingAddress.Ward, orderDto.ShippingAddress.DetailAddress);
        //var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

        order.UpdateStatus(  
            status: orderDto.Status);
    }
}
