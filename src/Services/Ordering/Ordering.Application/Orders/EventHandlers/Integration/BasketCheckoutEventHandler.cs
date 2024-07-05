using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderItem = Ordering.Domain.Models.OrderItem;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public  class BasketCheckoutEventHandler
        (ISender sender, ILogger<BasketCheckoutEvent> logger)
        : IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            // Todo: Create new order and start order fullfillment process
            logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
            
            // context.Message means Basket checkout event object
            var command = MapToCreateOrderCommand(context.Message);
           await  sender.Send(command);
        }
        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            // Create full order with incoming event data
            var addressDto = new AddressDto(message.CustomerName, message.Phone, message.Province, message.District, message.Ward, message.DetailAddress);
            var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
            var orderId = Guid.NewGuid();
            List<OrderItemDto> orderItems = new ();
            foreach (var item in  message.OrderItems)
            {
                orderItems.Add(new OrderItemDto(orderId, item.ProductId, item.Quantity, item.Price));
            }

            var orderDto = new OrderDto(
                Id: orderId,
                CustomerId: message.CustomerId,
                OrderName: "ORDER12",
                ShippingAddress: addressDto,
                BillingAddress: addressDto,
                Payment: paymentDto,
                Note: message.Note,
                DateOrder: DateTime.Now,
                Status: Ordering.Domain.Enums.OrderStatus.Pending,
                OrderItems: orderItems
                );
            // here should get this product information from the incoming request message from RabbitMQ 
            return new CreateOrderCommand(orderDto);
        }
    }
}
