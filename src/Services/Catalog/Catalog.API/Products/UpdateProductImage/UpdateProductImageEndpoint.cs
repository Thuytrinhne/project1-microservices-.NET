
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.UpdateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.UpdateProductImage
{
    public record UpdateProductImageRequest (IFormFile Image);
    public record UpdateProductImageResponse(bool IsSuccess);
    public class UpdateProductImageEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/products/{id}/image", async ([FromForm] UpdateProductImageRequest request, Guid id, ISender sender)
            =>
            {
                var command = new UpdateProductImageCommand(id, request.Image);
                var result = await sender.Send(command);
                return Results.Ok(result);

            })
                .DisableAntiforgery() //it need 
                .WithName("UpdateProductImage")
                .Produces<UpdateProductImageResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("UpdateProductImage")
                .WithDescription("UpdateProductImage");
        } 
    }
}
