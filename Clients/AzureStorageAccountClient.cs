using Azure.Identity;
using Azure.Storage.Blobs;

namespace AzureBatchEndpoint.Clients
{
    public record AzureStorageAccountOptions
    {
        public string TenantId { get; set; } = "fortedigital";
        public string BlobServiceUri { get; set; } = "https://testlabeling8469051483.blob.core.windows.net";
        public string ContainerName { get; set; } = "azureml-blobstore-776dfdfc-058f-488b-96cc-d0eb7797cb77";
        public string FilePath { get; set; } = "Diamond";
    }

    public class AzureStorageAccountClient
    {
        private readonly AzureStorageAccountOptions _azureStorageAccountOptions;
        private readonly BlobServiceClient _blobServiceClient;
        public AzureStorageAccountClient(AzureStorageAccountOptions azureStorageAccountOptions)
        {
            _azureStorageAccountOptions = azureStorageAccountOptions;
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                TenantId = _azureStorageAccountOptions.TenantId,
            });

            _blobServiceClient = new BlobServiceClient(new Uri(_azureStorageAccountOptions.BlobServiceUri), credential);
        }

        public async Task<string> UploadFileToAzureStorageAccountForPrediciton(MemoryStream inMemoryCsv, string filename) 
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_azureStorageAccountOptions.ContainerName);
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to the BlobClient to interact with the specific blob (file) with the desired path.
            var filepath = _azureStorageAccountOptions.FilePath + "/" + filename;
            var filePathIncFileName = filepath + ".csv";
            var blobClient = containerClient.GetBlobClient(filePathIncFileName);

            // Upload the content of the MemoryStream.
            await blobClient.UploadAsync(inMemoryCsv, true);

            return filepath;
        }

        public async Task<string> DownloadAndReadPredictionResult(string filepath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_azureStorageAccountOptions.ContainerName);
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to the BlobClient to interact with the specific blob (file) with the desired path.
            var filePathIncFileName = filepath + "_predictions.csv";
            var blobClient = containerClient.GetBlobClient(filePathIncFileName);

            // Download the blob's content to a MemoryStream.
            var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);

            // Reset the MemoryStream position to the beginning.
            memoryStream.Position = 0;

            // Read the content of the MemoryStream into a string variable.
            using var reader = new StreamReader(memoryStream);
            var header = reader.ReadLine();
            var values = reader.ReadLine();

            return values.Split(",")[0];
        }
    }
}
