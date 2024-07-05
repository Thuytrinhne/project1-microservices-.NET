
using Mapster;
using MediatR;
using User.API.Users.Account.CreateAddress;

namespace User.API.Users.Account.DeleteAddress
{
    public record DeleteAddressResponse (bool IsSuccess);
    public class DeleteAddressEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/users/{id}/addresses/{addressId}", async (Guid id, Guid addressId, ISender sender) =>
            {
                var command = new DeleteAddressCommand(id, addressId);
                var result =  await sender.Send(command);
                var response = result.Adapt<DeleteAddressResponse>();
                return Results.Ok(response);
            })
                .WithName("DeleteAddress")
                .Produces<DeleteAddressResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("DeleteAddress")
                .WithDescription("DeleteAddress");
        }
    }
}
