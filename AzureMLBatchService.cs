using FishHealth.StockPrediction;

namespace AzureBatchEndpoint
{
    public class AzureMLBatchService(AzureMLBatchClient azureMLBatchClient)
    {
        private readonly AzureMLBatchClient _azureMLBatchClient = azureMLBatchClient;

        public async Task<string> RunBatchEndpoint() 
        {
            var filepath = "";
            var filename = "";
            var jobId = await _azureMLBatchClient.InvokeBatchEndpoint(filepath, filename);

        }
    }
}
