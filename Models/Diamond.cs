namespace AzureBatchEndpoint.Models
{
    public class NotAppraisedDiamond
    {
        // 4Cs]
        public double Carat { get; set; }
        public string Clarity { get; set; }
        public string Colour { get; set; }
        public string Cut { get; set; }
    }

    public class Diamond : NotAppraisedDiamond
    {
        public int Price { get; set; }

        //// other
        //public string X { get; set; }
        //public string Y { get; set; }
        //public string Z { get; set; }
        //public string Depth { get; set; }
        //public string Table { get; set; }
    }
}