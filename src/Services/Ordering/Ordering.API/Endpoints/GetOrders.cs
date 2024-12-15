
using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;
using Ordering.Domain.ValueObjects;

namespace Ordering.API.Endpoints
{
    /*
        - Accepts pagination parameters
        - Constructs a GetOrdersQuery with these parameters
        - Retrieves the data and returns it in a paginated format
     */
    public record GetOrdersResponse (PaginationResult<OrderDto> PaginationResult);
    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/", async ([AsParameters]PaginationRequest request,  ISender sender, Guid ? CustomerId ,int ? StatusOrder = -1) =>
            {
                var customerId = CustomerId ?? Guid.Empty;
                var result = await  sender.Send(new GetOrdersQuery(request,  StatusOrder.Value, customerId));
                var response = result.Adapt<GetOrdersResponse>();
                return Results.Ok(response);
            })
             .WithName("GetOrders")
             .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Orders")
             .WithDescription("Get Orders");
        }
    
    }
}
