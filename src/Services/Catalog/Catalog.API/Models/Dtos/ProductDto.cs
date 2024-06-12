namespace Catalog.API.Models.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public string Title { get; set; } = default!;
        Category Category { get; set; }
        public string Description { get; set; } = default!;
        public CatalogImage Image { get; set; } = default!;
        public decimal Price { get; set; }
        public string Tags { get; set; } = default!;
        public int RatingCount { get; set; } = default!;
        public double AverageRating { get; set; } = default!;
    }
}
