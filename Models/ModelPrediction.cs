namespace AzureBatchEndpoint.Models
{
    public class ModelPrediction
    {
        public string ModelVersion { get; set; }
        public string ModelName { get; set; }
        public Diamond Diamond { get; set; }
        public DateTime DateOfPrediction { get; set; }
        public string jobId { get; set; }
        public string FilePath { get; set; }
        public string Prediction { get; set; }
        public string PredictionStatus { get; set; }
    }
}
