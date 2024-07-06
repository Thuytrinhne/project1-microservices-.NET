using Marten.Linq;
using Marten.Linq.QueryHandlers;
using Marten.Pagination;
using static System.Formats.Asn1.AsnWriter;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductQuery (string? Title, string? Name, int? PageNumber = 1, int? PageSize = 10)
        : IQuery<GetProductResult>;
    public record GetProductResult (List<GroupedProducts>  ProductDtos);

    internal class GetProductsHandler(IDocumentSession session )
        : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle( GetProductQuery  query, CancellationToken cancellationToken)
        {
            QueryStatistics stats = null;
            IEnumerable <Product> products;
            if (!string.IsNullOrEmpty(query.Name))
            {
                products = await session.Query<Product>()
              .Stats(out stats)
              .Where(p => p.Name.Contains(query.Name))
              .Take(query.PageSize.Value)
              .Skip(query.PageSize.Value * query.PageNumber.Value)
              .ToListAsync();
            }
            else if (string.IsNullOrEmpty(query.Title) )
            {
                 products = await session.Query<Product>()
                .Stats(out stats)
                .Take(query.PageSize.Value)
                .Skip(query.PageSize.Value * query.PageNumber.Value)
                .ToListAsync();
            }          
            else
            {
                 products = await session.Query<Product>()
               .Stats(out stats)
               .Where(p=>p.Title ==  query.Title)
               .Take(query.PageSize.Value)
               .Skip(query.PageSize.Value * query.PageNumber.Value)
               .ToListAsync();
            }    
                        var groupedProducts = products.GroupBy(p => p.Title)
                .Select(g => new GroupedProducts
                {
                    Title = g.Key,
                    Products = g.ToList().Adapt<List<ProductDto>>()    
                })
                .ToList();



            return new GetProductResult(groupedProducts);
        }
       
    }
    public class GroupedProducts
    {
        public string Title { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
