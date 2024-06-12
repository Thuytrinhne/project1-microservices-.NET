namespace Catalog.API.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public string Title { get; set; } = default!;
        public Category Category { get; set; } = new();
        public string Description { get; set; } = default!;
        public CatalogImage Image { get; set; }= default!;
        public decimal Price { get; set; }
        public string Tags { get; set; } = default!;
        public int RatingCount { get; set; } = default!;
        public double AverageRating { get; set; } = default!;


    }
    public class CatalogImage
    {
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
    }
}
