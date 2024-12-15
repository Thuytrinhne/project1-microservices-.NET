using Mapster;
using static User.API.Users.Account.GetAccount.GetAccountHandler;

namespace User.API.Users.Account.GetAddresses
{
    public record GetAccountResponse(UserDto User);
    public class GetAccountEndpoint : ICarterModule
    {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("/users/{id}", async (Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new GetAccountQuery(id));
                    var response = result.Adapt<GetAccountResponse>();
                    return Results.Ok(response);
                })
                 .WithName("GetAccount")
                 .Produces<GetAccountResponse>(StatusCodes.Status200OK)
                 .ProducesProblem(StatusCodes.Status400BadRequest)
                 .ProducesProblem(StatusCodes.Status404NotFound)
                 .WithSummary("GetAccount")
                 .WithDescription("GetAccount");
            }
        
    }
}
