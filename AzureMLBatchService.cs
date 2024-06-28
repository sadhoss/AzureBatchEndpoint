using FishHealth.StockPrediction;

namespace AzureBatchEndpoint
{
    public class AzureMLBatchService(AzureMLBatchClient azureMLBatchClient, AzureStorageAccountClient azureStorageAccountClient)
    {
        private readonly AzureMLBatchClient _azureMLBatchClient = azureMLBatchClient;
        private readonly AzureStorageAccountClient _azureStorageAccountClient = azureStorageAccountClient;

        public async Task<string?> RunBatchEndpoint() 
        {
            var filepath = "";
            var filename = "";
            var jobId = await _azureMLBatchClient.InvokeBatchEndpoint(filepath, filename);

            return jobId;
        }

        public async Task<ModelPrediction> GetBatchEndpointPrediction(string jobId)
        {

            var jobStatus = await _azureMLBatchClient.PingJobStatus(jobId);

            if (jobStatus == "Pending")
            {
                return new ModelPrediction()
                {
                    ModelName = "Not Found",
                    ModelVersion = "404",
                    ModelInputParameterOne = "One",
                    ModelInputParameterTwo = "Two",
                    DateOfPrediction = DateTime.Now,
                    Prediction = "",
                    PredictionStatus = jobStatus
                };
            }

            return new ModelPrediction()
            {
                ModelName = "Not Found",
                ModelVersion = "404",
                ModelInputParameterOne = "One",
                ModelInputParameterTwo = "Two",
                DateOfPrediction = DateTime.Now,
                Prediction = await _azureStorageAccountClient.DownloadAndReadPredictionResult(),
                PredictionStatus = jobStatus
            };
        }
    }
}
