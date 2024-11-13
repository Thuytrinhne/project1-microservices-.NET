

using Mapster;
using User.API.Users.Auth.Register;

namespace User.API.Users.Auth.Login
{
    public record LoginRequest(string Email, string Password);     
    public record LoginResponse(UserDto User, string Token);
    public class LoginEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async (LoginRequest request, ISender sender) =>
            {
                var command = request.Adapt<LoginCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<LoginResponse>();
                return Results.Ok(response);
            })
                .WithName("Login")
                .Produces<LoginResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Login")
                .WithDescription("Login");
        }
    }
}
