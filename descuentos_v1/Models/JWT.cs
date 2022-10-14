namespace WSDISCOUNT.Models
{
    public class JWT
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public double Time { get; set; }
    }
}
