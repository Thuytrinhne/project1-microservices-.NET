namespace Ordering.Domain.ValueObjects
{
    public record OrderItemId // create value object for these new strongly type IDs
    {
        public Guid Value { get; }
        private OrderItemId(Guid value) => Value = value;
        public static OrderItemId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("OrderItemId cannot be empty");
            }
            return new OrderItemId(value);
        }
    }
}
