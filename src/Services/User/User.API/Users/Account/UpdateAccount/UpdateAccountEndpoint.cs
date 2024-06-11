
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using User.API.Users.Auth.ForgotPassword;

namespace User.API.Users.Account.UpdateAccount
{
    public record UpdateAccountRequest(string Name ,IFormFile Image ,int Gender , DateTime DOB );
  
    public record UpdateAccountResponse (UserDto User);
    public class UpdateAccountEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
           app.MapPatch("/accounts/{id}/general-infor", async ([FromForm]UpdateAccountRequest request, Guid id, ISender sender) =>
           {
               var updateAccountDTO = new UpdateAccountDto
               { Name = request.Name, Gender= request.Gender, DOB = request.DOB, Image = request.Image}; 
               var command = new UpdateAccountCommand(id, updateAccountDTO);
               var result = await sender.Send(command);
               var response = result.Adapt<UpdateAccountResponse>();
               return Results.Ok(response);
           })
                .DisableAntiforgery() //it need 
                 .WithName("UpdateAccount")
                .Produces<UpdateAccountResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("UpdateAccount")
                .WithDescription("UpdateAccount");
        }
    }
}
