namespace AzureBatchEndpoint.Models
{
    public class BaseModelPrediction 
    {
        public string ModelVersion { get; set; }
        public string ModelName { get; set; }
        public DateTime DateOfPrediction { get; set; }
        public string jobId { get; set; }
        public string FilePath { get; set; }
        public string PredictionStatus { get; set; }
    }

    public class PreModelPrediction : BaseModelPrediction
    {
        public UnpraisedDiamond UnpraisedDiamond { get; set; }
    }

    public class PostModelPrediction : BaseModelPrediction
    {
        public Diamond Diamond { get; set; }
        public double Prediction { get; set; }
    }
}
