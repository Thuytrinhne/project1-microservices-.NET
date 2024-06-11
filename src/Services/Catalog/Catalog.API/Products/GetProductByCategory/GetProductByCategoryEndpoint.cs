
using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductsByCategoryResponse (IEnumerable<Product> Products);
    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (Guid  category, ISender sender) =>
            {
                var result = await  sender.Send(new GetProductsByCategoryQuery(category));
                var response =  result.Adapt<GetProductsByCategoryResponse>();
                return Results.Ok(response);
            })
             .WithName("GetProductsByCategory")
             .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Products By Category")
             .WithDescription("Get Products By Category");
        }
    }
}
