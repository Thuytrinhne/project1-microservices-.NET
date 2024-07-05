
using Basket.API.Basket.GetBasket;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest (ShoppingCart Cart);
    public record StoreBasketResponse (Guid UserId );
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async ([FromBody]StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.UserId}", response);

            })
             .WithName("StoreBasket")
             .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Store Basket")
             .WithDescription("Store Basket");
        }
    }
}
