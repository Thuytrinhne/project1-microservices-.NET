namespace Ordering.Domain.ValueObjects
{
    public record ProductId // create value object for these new strongly type IDs
    {
        public Guid Value { get; }
        private ProductId(Guid value) => Value = value;
        public static ProductId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("ProductId cannot be empty");
            }
            return new ProductId(value);
        }
    }
}
