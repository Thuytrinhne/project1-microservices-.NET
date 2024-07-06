using BuildingBlocks.Pagination;
namespace Ordering.Application.Orders.Queries.GetOrders
{
    public record  GetOrdersQuery (PaginationRequest PaginationRequest, int StatusOrder)
        : IQuery<GetOrdersResult>;
    public record GetOrdersResult (PaginationResult<OrderDto> PaginationResult);
    
}
