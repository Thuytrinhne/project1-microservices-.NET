
using Mapster;
using User.API.Users.Account.UpdateAccount;

namespace User.API.Users.Account.CreateAddress
{
    public record CreateAddressRequest(CreateUserAddressDto UserAddress);
    public record CreateAddressResponse(bool IsSuccess);

    public class CreateAddressEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/accounts/{id}/addresses", async (Guid id, CreateAddressRequest request, ISender sender) =>
            {
                var command = new CreateAddressCommand (  id, request.UserAddress );
                var result = await sender.Send(command);
                var response = result.Adapt<CreateAddressResponse>();
                return Results.Ok(response);
            })
             
                 .WithName("CreateAddress")
                .Produces<CreateAddressResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("CreateAddress")
                .WithDescription("CreateAddress");
        }
    }
}
