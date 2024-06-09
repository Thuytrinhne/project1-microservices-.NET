
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace User.API.Users.Auth.SendOTP
{
    public record SendOTPCommand (string Email) :ICommand<SendOTPResult>;
    public record SendOTPResult(bool IsSuccess);

    public class SendOTPCommandHandler
        (EmailVerificationService _service, UserManager<ApplicationUser> _userManager, IEmailSender _emailSender)
        : ICommandHandler<SendOTPCommand, SendOTPResult>
    {
        
        public async Task<SendOTPResult> Handle(SendOTPCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user is not null)
            {
                return new  SendOTPResult(false);
            }
            string otp = GenerateOTP();
            await SendOTPEmail(command.Email, otp);
            await  _service.CreateAsync(new EmailVerification { OTP = otp, Email = command.Email, ExpiryTime = DateTime.UtcNow.AddMinutes(Authorization.OTPExpiredTimeInMinutes)});
            return new SendOTPResult(true);


        }
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        private async Task SendOTPEmail(string email, string otp)
        {
            string subject = "Comestic Store- Sending OTP";
            string message = $"Your OTP is {otp}.\n Note that this OTP is valid within {Authorization.OTPExpiredTimeInMinutes} mins.";
            await _emailSender.SendEmailAsync(email, subject, message);
        }
    }
}
