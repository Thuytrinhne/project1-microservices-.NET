namespace Ordering.Application.Orders.Queries.GetOrderByName
{
    public record GetOrdersByIdQuery (Guid Id)
        : IQuery<GetOrdersByIdResult>;
    public record GetOrdersByIdResult(IEnumerable<OrderDto> Orders); 
}
