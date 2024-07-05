using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery (Guid userId) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);
    internal class GetBasketQueryHandler (IBasketRepository repository)
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async  Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket  = await  repository.GetBasket(query.userId);
            return new GetBasketResult(basket);
        }
    }
}
