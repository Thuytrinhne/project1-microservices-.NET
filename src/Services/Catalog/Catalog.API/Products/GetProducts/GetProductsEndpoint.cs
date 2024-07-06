
using Catalog.API.Products.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.GetProducts
{

    public record GetProductsResponseByTitle(List<GroupedProducts> ProductDtos);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ( ISender sender, string? Title, string ?Name, int? PageNumber = 0, int? PageSize = 20) =>
            {
                var query = new GetProductQuery(Title, Name ,PageNumber, PageSize);
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductsResponseByTitle>();
                return Results.Ok(response);
            })
                .WithName("GetProducts")
                .Produces<GetProductsResponseByTitle>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("GetProducts")
                .WithDescription("Get Products");
        }
    }
}
