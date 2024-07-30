using AzureBatchEndpoint.Clients;
using AzureBatchEndpoint.Models;
using System.Text;

namespace AzureBatchEndpoint
{
    public class MLService(AzureMLBatchClient azureMLBatchClient, AzureStorageAccountClient azureStorageAccountClient)
    {
        private readonly AzureMLBatchClient _azureMLBatchClient = azureMLBatchClient;
        private readonly AzureStorageAccountClient _azureStorageAccountClient = azureStorageAccountClient;

        public async Task<PreModelPrediction> Predict(UnpraisedDiamond unpraisedDiamond) 
        {
            var filepath = await ConvertAndUpload(unpraisedDiamond);
            var jobId = await _azureMLBatchClient.InvokeBatchEndpoint(filepath);

            return new PreModelPrediction() 
            {
                ModelName = "Not Found",
                ModelVersion = "404",
                UnpraisedDiamond = unpraisedDiamond,
                FilePath = filepath,
                DateOfPrediction = DateTime.Now,
                PredictionStatus = "Pending",
                jobId = jobId ?? ""
            };
        }

        private async Task<string> ConvertAndUpload(UnpraisedDiamond unpraisedDiamond)
        {
            using var csvData = ConvertAndFormatData(unpraisedDiamond);

            var filename = $"diamond_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";

            var filepath = await _azureStorageAccountClient.UploadFileToAzureStorageAccountForPrediction(csvData, filename);

            return filepath;
        }

        private static MemoryStream ConvertAndFormatData(UnpraisedDiamond unpraisedDiamond)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("carat,cut,color,clarity,price");
            csvContent.Append($"{unpraisedDiamond.Carat:0.0},{unpraisedDiamond.Cut},{unpraisedDiamond.Colour},{unpraisedDiamond.Clarity},0");

            var inMemoryCsv = new MemoryStream();
            var streamWriter = new StreamWriter(inMemoryCsv, Encoding.UTF8);
            streamWriter.Write(csvContent.ToString());
            streamWriter.Flush();

            // Reset the position of the MemoryStream to the beginning
            inMemoryCsv.Position = 0;

            return inMemoryCsv;
        }

        public async Task<PostModelPrediction> GetPrediction(string jobId, string filePath)
        {
            var predictionStatus = await _azureMLBatchClient.PingJobStatus(jobId);

            if (predictionStatus != "Completed")
                return new PostModelPrediction() { jobId = jobId, FilePath = filePath, PredictionStatus = predictionStatus };

            var modelprediction = await _azureStorageAccountClient.DownloadAndReadPredictionResult(filePath);
            modelprediction.jobId = jobId;
            modelprediction.PredictionStatus = predictionStatus;
            return modelprediction;
        }
    }
}
