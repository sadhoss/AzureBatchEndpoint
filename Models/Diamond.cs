namespace AzureBatchEndpoint.Models
{
    public class Diamond
    {
        public string Price { get; set; }

        // 4Cs
        public double Carat { get; set; }
        public string Clearity { get; set; }
        public string Colour { get; set; }
        public string Cut { get; set; }

        // other
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public string Depth { get; set; }
        public string Table { get; set; }
    }
}