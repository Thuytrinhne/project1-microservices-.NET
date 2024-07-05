namespace Ordering.Application.Orders.Commands.CreateOrder;
public class CreateOrderHandler(IAppDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        //create Order entity from command object
        //save to database
        //return result 

        var order = CreateNewOrder(command.Order);

        await  dbContext.Orders.AddAsync(order);
     
            await dbContext.SaveChangesAsync(cancellationToken);
       
        
        return new CreateOrderResult(order.Id.Value);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.CustomerName, orderDto.ShippingAddress.Phone, orderDto.ShippingAddress.Province, orderDto.ShippingAddress.District, orderDto.ShippingAddress.Ward, orderDto.ShippingAddress.DetailAddress);
        var billingAddress = Address.Of(orderDto.ShippingAddress.CustomerName, orderDto.ShippingAddress.Phone, orderDto.ShippingAddress.Province, orderDto.ShippingAddress.District, orderDto.ShippingAddress.Ward, orderDto.ShippingAddress.DetailAddress);

        var newOrder = Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(orderDto.CustomerId),
                orderName: OrderName.Of(orderDto.OrderName),
                shippingAddress: shippingAddress,
                billingAddress: billingAddress,
                payment: Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod)
                );

        foreach (var orderItemDto in orderDto.OrderItems)
        {
            newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }
        return newOrder;
    }
}
