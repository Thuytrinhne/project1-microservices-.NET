using Ordering.Application.Orders.Queries.GetOrderByName;
namespace Ordering.API.Endpoints
{
    /*
        - Accept  a name parameter
        - Constructs a GetOrdersByNameQuery
        - Retrieves and returns matching orders 
     */
    public record GetOrdersByIdResponse(OrderDto Order);
    public class GetOrdersById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByIdQuery(id));
                var response = result.Adapt<GetOrdersByIdResponse>();
                return Results.Ok(response);
            })
             .WithName("GetOrdersById")
             .Produces<GetOrdersByIdResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Orders By Id")
             .WithDescription("Get Orders By Id");
        }
    }
}
