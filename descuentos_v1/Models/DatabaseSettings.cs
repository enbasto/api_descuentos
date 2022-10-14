namespace WSDISCOUNT.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionDiscount { get; set; } = null!;
        public string CollectionSales { get; set; } = null!;
        public string CollectionUsers { get; set; } = null!;
    }
}
