namespace AzureBatchEndpoint
{
    public class ModelPrediction
    {
        public string ModelVersion { get; set; }
        public string ModelName { get; set; }
        public string ModelInputParameterOne { get; set; }
        public string ModelInputParameterTwo { get; set; }
        public DateTime DateOfPrediction { get; set; }
        public string jobId { get; set; }
        public string FileName { get; set; }
        public string Prediction { get; set; }
        public string PredictionStatus { get; set; }
    }
}
