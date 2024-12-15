

using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.UpdateProductImage
{
    public record UpdateAccountImageRequest(IFormFile Image);
    public record UpdateAccountImageResponse(bool IsSuccess);
    public class UpdateAccountImageEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/users/{id}/image", async ([FromForm] UpdateAccountImageRequest request, Guid id, ISender sender)
            =>
            {
                var command = new UpdateAccountImageCommand(id, request.Image);
                var result = await sender.Send(command);
                return Results.Ok(result);

            })
                .DisableAntiforgery() //it need 
                .WithName("UpdateAccountImage")
                .Produces<UpdateAccountImageResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("UpdateAccountImage")
                .WithDescription("UpdateAccountImage");
        } 
    }
}
