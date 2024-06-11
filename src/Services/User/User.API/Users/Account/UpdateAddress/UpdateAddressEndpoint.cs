
using Mapster;
using User.API.Users.Account.CreateAddress;

namespace User.API.Users.Account.UpdateAddress
{
    public record UpdateAddressRequest ( UpdateUserAddressDto UserAddress);
    public record UpdateAddressResponse (UserAddressDto UserAddress);
    public class UpdateAddressEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/accounts/{id}/addresses", async (Guid id, UpdateAddressRequest request, ISender sender ) =>
            {
                var command = new UpdateAddressCommand(id, request.UserAddress);
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateAddressResponse>();
                return Results.Ok(response);
            })
                .WithName("UpdateAddress")
                .Produces<UpdateAddressResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("UpdateAddress")
                .WithDescription("UpdateAddress");
        }
    }
}
