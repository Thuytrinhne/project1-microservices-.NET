
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using User.API.Users.Auth.Login;

namespace User.API.Users.Auth.ForgotPassword
{
	public record ForgotPasswordRequest(string Email);
	public record ForgotPasswordResponse(bool IsSuccess);

    public class ForgotPasswordEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/forgot-password", async (ForgotPasswordRequest request, ISender sender) =>
            {
                var command = request.Adapt<ForgotPasswordCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<ForgotPasswordResponse>();
                return Results.Ok(response);

            })
                .WithName("ForgotPassword")
                .Produces<ForgotPasswordResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("ForgotPassword")
                .WithDescription("ForgotPassword");
        }
    }
}
