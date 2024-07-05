
using Mapster;
using User.API.Users.Account.UpdateAccount;

namespace User.API.Users.Account.CreateAddress
{
    public record CreateAddressRequest(
        string Name,
    string Phone ,
     int Default ,
     string Province ,
     string District ,
     string Ward ,
     string DetailAddress);
    public record CreateAddressResponse(bool IsSuccess);

    public class CreateAddressEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/users/{id}/addresses", async (Guid id, CreateAddressRequest request, ISender sender) =>
            {
                var command = new CreateAddressCommand (id, request.Phone, request.Name, request.Default, request.Province,
                    request.District, request.Ward, request.DetailAddress);
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
