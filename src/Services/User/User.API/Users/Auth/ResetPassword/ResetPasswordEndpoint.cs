
using Mapster;
using User.API.Users.Auth.Register;

namespace User.API.Users.Auth.ResetPassword
{
    public record ResetPasswordRequest (ResetPasswordDto ResetPasswordDto);
    public record ResetPasswordResponse(bool IsSuccess);
    public class ResetPasswordEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/auth/reset-password", async (ResetPasswordRequest request, ISender sender) =>
            {
                var command = request.Adapt<ResetPasswordCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<ResetPasswordResponse>();
                return Results.Ok(response);

            })
                .WithName("ResetPassword")
                .Produces<ResetPasswordResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Reset Password")
                .WithDescription("Reset Password");
        }
    }
}
