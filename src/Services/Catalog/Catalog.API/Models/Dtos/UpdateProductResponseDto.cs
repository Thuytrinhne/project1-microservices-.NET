namespace Catalog.API.Models.Dtos
{
    public class UpdateProductResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        Category Category { get; set; } = new();
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public string Tags { get; set; } = default!;
        public int RatingCount { get; set; } = default!;
        public double AverageRating { get; set; } = default!;
    }
}
