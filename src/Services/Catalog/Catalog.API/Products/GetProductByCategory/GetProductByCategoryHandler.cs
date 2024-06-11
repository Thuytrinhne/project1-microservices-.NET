
namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductsByCategoryQuery (Guid Category)
        :IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<ProductDto> Products);

    internal class GetProductByCategoryQueryHandler (IDocumentSession session)
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .Where(p => p.Category.Id == query.Category)
                .ToListAsync(cancellationToken);
         
            return new GetProductsByCategoryResult(products.Adapt<List<ProductDto>>());
        }
    }
}
