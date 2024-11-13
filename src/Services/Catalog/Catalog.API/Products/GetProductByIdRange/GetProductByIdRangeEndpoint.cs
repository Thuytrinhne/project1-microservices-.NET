
using Catalog.API.Products.GetProductById;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.GetProductByIdRange
{
    public record GetProductByIdRangeResponse(List<ProductDto> Products);
    public class GetProductByIdRangeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products/search", async ([FromBody] List<Guid> idRange, ISender sender) => {
                var result = await  sender.Send(new GetProductByIdRangeQuery(idRange));
                var response = result.Adapt<GetProductByIdRangeResponse>();
                return Results.Ok(response);
            })
             .WithName("GetProductByIdRange")
             .Produces<GetProductByIdRangeResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Product By Id")
             .WithDescription("Get Product By Id");
        }
    }
}
