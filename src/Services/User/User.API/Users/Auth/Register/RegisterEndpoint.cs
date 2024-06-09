

using Mapster;
using MediatR;

namespace User.API.Users.Auth.Register
{
    public record RegisterRequest(RegisterDto User);
    public record RegisterResponse(bool IsSuccess);
    public class RegisterEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/register", async (RegisterRequest request,  ISender sender) =>
            {
                var command = request.Adapt<RegisterCommand>();
                var result = await sender.Send(command);
                var response =  result.Adapt<RegisterResponse>();
                return Results.Created($"/auth/login", response);

            })
                .WithName("Register")
                .Produces<RegisterResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Register")
                .WithDescription("Register");

        }
    }
}
