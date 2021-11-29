namespace Medipharmacy.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Metric { get; set; }
        public string Size { get; set; }
        public string Unit { get; set; }
        public string Image { get; set; }
        public bool InStock { get; set; } = false;
        public int Remaining { get; set; }

        public ProductViewModel()
        {
        }

        public ProductViewModel(int productId, string name, string description, string price, string metric, string size, string unit, string image, bool inStock, int remaining)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            Metric = metric;
            Size = size;
            Unit = unit;
            Image = image;
            InStock = inStock;
            Remaining = remaining;

        }
    }
}
