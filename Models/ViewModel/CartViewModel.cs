namespace StyleSphere.ViewModels
{
    public class CartViewModel
    {
        // CartItem table fields
        public int CartId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // From Product table (joined using navigation property)
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public int Price { get; set; }

        public int TotalPrice => Quantity * Price;
    }
}
