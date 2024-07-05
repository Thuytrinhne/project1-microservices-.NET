
namespace Ordering.Application.Extensions
{
    public static  class OrderExtensions
    {

        public static IEnumerable<OrderDto> ToOrderDtoList (this IEnumerable<Order> orders)
        {

            return orders.Select(order => new OrderDto(
                     Id: order.Id.Value,
                     CustomerId: order.CustomerId.Value,
                     OrderName: order.OrderName.Value,
                     ShippingAddress: new AddressDto(
                         order.ShippingAddress.CustomerName,
                         order.ShippingAddress.Phone,
                         order.ShippingAddress.Province,
                         order.ShippingAddress.District,
                         order.ShippingAddress.Ward,
                         order.ShippingAddress.DetailAddress
                         ),
                       BillingAddress: new AddressDto(
                         order.ShippingAddress.CustomerName,
                         order.ShippingAddress.Phone,
                         order.ShippingAddress.Province,
                         order.ShippingAddress.District,
                         order.ShippingAddress.Ward,
                         order.ShippingAddress.DetailAddress
                         ),
                       Payment: new PaymentDto
                       (
                         order.Payment.CardName!,
                         order.Payment.CardNumber,
                         order.Payment.Expiration,
                         order.Payment.CVV,
                         order.Payment.PaymentMethod.Value
                       ),
                       Status: order.Status,
                       Note: order.Note,
                       DateOrder: DateTime.Now,
                       OrderItems: order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()

                     ));
            
        }


        public static  OrderDto ToOrderDto(this Order  order)
        {
            return DtoFromOrder(order);
        }

        private static OrderDto DtoFromOrder(Order order)
        {
            return new OrderDto(
                                Id: order.Id.Value,
                                CustomerId: order.CustomerId.Value,
                                OrderName: order.OrderName.Value,
                                ShippingAddress: new AddressDto(order.ShippingAddress.CustomerName, order.ShippingAddress.Phone, order.ShippingAddress.Province, order.ShippingAddress.District, order.ShippingAddress.Ward, order.ShippingAddress.DetailAddress ),
                                BillingAddress: new AddressDto(order.ShippingAddress.CustomerName, order.ShippingAddress.Phone, order.ShippingAddress.Province, order.ShippingAddress.District, order.ShippingAddress.Ward, order.ShippingAddress.DetailAddress),
                                Payment: new PaymentDto(order.Payment.CardName!, order.Payment.CardNumber, order.Payment.Expiration, order.Payment.CVV, order.Payment.PaymentMethod.Value),
                                Status: order.Status,
                                Note: order.Note,
                                DateOrder: DateTime.Now,
                                OrderItems: order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()
                            );
        }
    }
}
