
using Catalog.API.Products.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest(int ? PageNumber = 1, int ? PageSize = 10);

    public record GetProductsResponseByTitle(List<GroupedProducts> ProductDtos);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters]GetProductsRequest request ,  ISender sender) =>
            {
                var query = request.Adapt<GetProductQuery>();
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductsResponseByTitle>();
                return Results.Ok(response);
            })
                .WithName("GetProducts")
                .Produces<CreateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("GetProducts")
                .WithDescription("Get Products");
        }
    }
}
