namespace User.API.Enums
{
    public class Authorization
    {
        public enum Roles
        {
            Admin,
            Staff,
            User
        }
        public const Roles DefaultRole = Roles.User;
        public const int OTPExpiredTimeInMinutes = 20;
        public const double  ResetPasswordExpiredTimeInMinutes = 20;
        public const double AccessTokenExpiredTimeInMinutes = 60;

    }
}
