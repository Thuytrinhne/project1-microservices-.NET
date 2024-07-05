
namespace BuildingBlocks.Messaging.Events
{
    public record BasketCheckoutEvent :IntegrationEvent
    {
        public Guid CustomerId { get; set; } = default!;
        public decimal TotalPrice { get; set; } = default!;

        // Shipping and BillingAddress
        public string CustomerName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Province { get; set; } = default!;
        public string District { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string DetailAddress { get; set; } = default!;


        // Payment
        public string CardName { get; set; } = default!;
        public string CardNumber { get; set; } = default!;
        public string Expiration { get; set; } = default!;
        public string CVV { get; set; } = default!;
        public int PaymentMethod { get; set; } = default!;

        public string Note { get;  set; }
        public List<OrderItem> OrderItems { get; set; } = new();


    }
    public class OrderItem
    {
        public OrderItem(Guid productId, int quantity, decimal price)
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public Guid ProductId { get;  set; } = default!;
        public int Quantity { get;  set; } = default!;
        public decimal Price { get;  set; } = default!;

    }



}
