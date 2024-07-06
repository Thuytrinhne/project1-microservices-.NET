
using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints
{
    /*
        - Accepts a UpdateOrderRequest
        - Maps the request to an UpdateOrderCommand
        - Sends the command for processing
        - Returns a success or error response based on the outcome 
     */
    public record UpdateOrderRequest (OrderDto Order);
    public record UpdateOrderResponse (bool IsSuccess); 
    public class UpdateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders/{id}", async (Guid id, UpdateOrderRequest request, ISender sender) =>
            {
                var command = new UpdateOrderCommand(id, request.Order);
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateOrderResponse>();
                return Results.Ok(response);

            })
             .WithName("UpdateOrder")
             .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Update Order")
             .WithDescription("Update Order");
        }
    }
}
