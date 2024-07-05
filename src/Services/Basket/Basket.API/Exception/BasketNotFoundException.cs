
namespace Basket.API.Exception
{
    public class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(Guid userId) : base("Basket", userId)
        {
        }
    }
}
