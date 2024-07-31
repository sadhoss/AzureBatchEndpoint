using Swashbuckle.AspNetCore.Annotations;

namespace AzureBatchEndpoint.Models
{
    public class UnpraisedDiamond
    {
        // 4Cs]
        [SwaggerSchema(Description = "Example = 0.5")]
        public double Carat { get; set; }
        [SwaggerSchema(Description = "Example = VVS2")]
        public string Clarity { get; set; }
        [SwaggerSchema(Description = "Example = E")]
        public string Colour { get; set; }
        [SwaggerSchema(Description = "Example = Fair")]
        public string Cut { get; set; }
    }

    public class Diamond : UnpraisedDiamond
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