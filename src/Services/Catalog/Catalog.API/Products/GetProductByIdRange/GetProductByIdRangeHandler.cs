using Catalog.API.Extensions;
using Marten;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdRangeQuery (List<Guid> IdRange) : IQuery<GetProductByIdRangeResult>;
    public record GetProductByIdRangeResult(List<ProductDto> Products);

    internal class GetProductByIdRangeHandler(IDocumentSession session)
        : IQueryHandler<GetProductByIdRangeQuery, GetProductByIdRangeResult>
    {
        public async Task<GetProductByIdRangeResult> Handle(GetProductByIdRangeQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .Where(x => x.Id.IsOneOf(query.IdRange.ToArray()))
                .ToListAsync();
            if (products is null)
                throw new ProductNotFoundException(query.IdRange[0]);
           
            return new GetProductByIdRangeResult(products.Adapt<List<ProductDto>>());
        }
    }
}
