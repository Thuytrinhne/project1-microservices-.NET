namespace Ordering.Domain.ValueObjects
{
    public  record Address // record: immutable
    {
        public string CustomerName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Province { get; set; } = default!;
        public string District { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string DetailAddress { get; set; } = default!;
        protected Address()
        {
            // required for EF
        }

        private Address(string customerName, string phone, string? province, string district, string ward, string detailAddress)
        {
            CustomerName = customerName;
            Phone = phone;
            Province = province;
            District = district;
            Ward = ward;
            DetailAddress = detailAddress;
        }
        public static Address Of(string customerName, string phone, string? province, string district, string ward, string detailAddress)
        {
            //ArgumentException.ThrowIfNullOrWhiteSpace(customerName);
            //ArgumentException.ThrowIfNullOrWhiteSpace(phone);

            return new Address(customerName, phone, province, district, ward, detailAddress);
        }
    }
}
