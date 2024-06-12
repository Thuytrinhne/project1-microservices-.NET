
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProductById;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(
        string ? Name,
      string ? Title ,
      Guid? CategoryId ,
      string? Description ,
      decimal? Price ,
      string? Tags);
    public record UpdateProductResponse(ProductDto Product);

    public class UpdateProductEndpoint  : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/products/{id}", async ( UpdateProductRequest request,Guid id,  ISender sender)
            =>{

                var command = new
                UpdateProductCommand(id,request.Name,  request.Title, request.CategoryId.Value, request.Description, request.Price.Value, request.Tags);
                
               var result = await sender.Send(command);
                 
               return Results.Ok(result);
               

            }) 
              .WithName("UpdateProduct")
             .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Update Product")
             .WithDescription("Update Product");
        }
    }
}
