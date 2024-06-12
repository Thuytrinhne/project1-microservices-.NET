using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(
        string Name, 
    string Title,
    Guid CategoryId,
    string Description,
    IFormFile Image,
    decimal Price ,
     string Tags );
    public record CreateProductResponse(Guid ProductId);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async ([FromForm]CreateProductRequest request, ISender sender) =>
            {
              //  var command = request.Adapt<CreateProductCommand>();
                var command = new CreateProductCommand(
                    request.Name,
                    request.Title,
                     request.CategoryId,
                     request.Description,
                    request.Image,
                  request.Price,
                  request.Tags

                );
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProductResponse>();
                return Results.Created($"/products/{response.ProductId}", response);
            })
                .DisableAntiforgery() //it need 
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
