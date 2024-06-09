
namespace Basket.API.Exception
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string message) : base("User", message)
        {
        }
    }
}
