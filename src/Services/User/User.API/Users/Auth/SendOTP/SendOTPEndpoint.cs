
using Mapster;
using User.API.Users.Auth.Login;

namespace User.API.Users.Auth.SendOTP
{
    public record SendOTPRequest (string Email);
    public record SendOTPResponse (bool IsSuccess);
    public class SendOTPEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/otps", async (SendOTPRequest request, ISender sender) =>
            {
                var command = request.Adapt<SendOTPCommand>();
                var result = await  sender.Send(command);
                var response = result.Adapt<SendOTPResponse>();
                return Results.Ok(response);
            })
                .WithName("SendOTP")
                .Produces<SendOTPResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("SendOTP")
                .WithDescription("SendOTP");
        }
    }
}
