namespace StyleSphere.Models.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? BuyImage { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
