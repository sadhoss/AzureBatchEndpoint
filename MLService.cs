using AzureBatchEndpoint.Clients;
using AzureBatchEndpoint.Models;
using System.Text;

namespace AzureBatchEndpoint
{
    public class MLService(AzureMLBatchClient azureMLBatchClient, AzureStorageAccountClient azureStorageAccountClient)
    {
        private readonly AzureMLBatchClient _azureMLBatchClient = azureMLBatchClient;
        private readonly AzureStorageAccountClient _azureStorageAccountClient = azureStorageAccountClient;

        public async Task<ModelPrediction> Predict(Diamond diamond) 
        {
            var filepath = await ConvertAndUpload(diamond);

            // depending of network speed, fileupload might not be done before model invocation
            await Task.Delay(2000); // 2 seconds

            var jobId = await _azureMLBatchClient.InvokeBatchEndpoint(filepath);

            return new ModelPrediction() 
            {
                ModelName = "Not Found",
                ModelVersion = "404",
                Diamond = diamond,
                FilePath = filepath,
                DateOfPrediction = DateTime.Now,
                Prediction = "",
                PredictionStatus = "Pending",
                jobId = jobId ?? ""
            };
        }

        private async Task<string> ConvertAndUpload(Diamond diamond)
        {
            using var csvData = ConvertAndFormatData(diamond);

            var filename = $"Diamond_{DateTime.Now}";

            var filepath = await _azureStorageAccountClient.UploadFileToAzureStorageAccountForPrediciton(csvData, filename);

            return filepath;
        }

        private static MemoryStream ConvertAndFormatData(Diamond diamond)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Price,Carat,Clarity,Colour,Cut");
            csvContent.AppendLine($"{diamond.Price},{diamond.Carat},{diamond.Clearity},{diamond.Colour},{diamond.Cut}");

            var inMemoryCsv = new MemoryStream();
            using var streamWriter = new StreamWriter(inMemoryCsv, Encoding.UTF8);
            streamWriter.Write(csvContent.ToString());
            streamWriter.Flush();

            // Reset the position of the MemoryStream to the beginning
            inMemoryCsv.Position = 0;

            return inMemoryCsv;
        }

        public async Task<ModelPrediction> GetBatchEndpointPrediction(ModelPrediction modelPrediction)
        {
            modelPrediction.PredictionStatus = await _azureMLBatchClient.PingJobStatus(modelPrediction.jobId);

            if (modelPrediction.PredictionStatus != "Completed")
                return modelPrediction;

            modelPrediction.Prediction = await _azureStorageAccountClient.DownloadAndReadPredictionResult(modelPrediction.FilePath);
            return modelPrediction;
        }
    }
}
