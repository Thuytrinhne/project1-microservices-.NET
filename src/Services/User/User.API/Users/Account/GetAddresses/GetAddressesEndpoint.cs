using Mapster;
using static User.API.Users.Account.GetAddresses.GetAddressesHandler;

namespace User.API.Users.Account.GetAddresses
{
    public record GetAddressesResponse(IEnumerable<UserAddressDto> ListAddress);
    public class GetAddressesEndpoint : ICarterModule
    {
       
       
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("/users/{id}/addresses", async (Guid id, ISender sender,Guid ? addressId, int ? defaultAddress = -1) =>
                {
                    var result = await sender.Send(new GetAddressesQuery(id, addressId??Guid.Empty, defaultAddress.Value));
                    var response = result.Adapt<GetAddressesResponse>();
                    return Results.Ok(response);
                })
                 .WithName("GetAddresses")
                 .Produces<GetAddressesResponse>(StatusCodes.Status200OK)
                 .ProducesProblem(StatusCodes.Status400BadRequest)
                 .ProducesProblem(StatusCodes.Status404NotFound)
                 .WithSummary("GetAddresses")
                 .WithDescription("GetAddresses");
            }
        
    }
}
