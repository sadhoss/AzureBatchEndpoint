using Azure.Identity;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FishHealth.StockPrediction
{
    public record AzureMLBatchClientOptions 
    {
        public string SubscriptionId {  get; set; } = "--";
        public string ResourceGroupName { get; set; } = "--";
        public string WorkspaceName { get; set; } = "--";
        public string Datastore { get; set; } = "--";
        public string EndpointName { get; set; } = "--";
        public string ApiVersion { get; set; } = "2023-04-01";
        public string EndpointUri { get; set; } = "https://<EndpointName>.norwayeast.inference.ml.azure.com/jobs";
    }

    public class AzureMLBatchClient()
    {
        private readonly AzureMLBatchClientOptions _azureMLBatchClientOptions = new();

        public async Task<string?> InvokeBatchEndpoint(string filePath, string fileName) 
        {
            using var batchEndpointClient = await InitializeHttpClient();

            var uriFolderPath = $"azureml://subscriptions/{_azureMLBatchClientOptions.SubscriptionId}/resourcegroups/{_azureMLBatchClientOptions.ResourceGroupName}/workspaces/{_azureMLBatchClientOptions.WorkspaceName}/datastores/{_azureMLBatchClientOptions.Datastore}/paths/{filePath}";

            // Prepare the request body
            var requestBody = new
            {
                properties = new
                {
                    InputData = new
                    {
                        HarvestDeviation = new
                        {
                            JobInputType = "UriFile",
                            Uri = uriFolderPath + fileName
                        }
                    },
                    OutputData = new
                    {
                        score = new
                        {
                            JobOutputType = "UriFile",
                            Uri = uriFolderPath
                        }
                    }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            // Invoke the batch endpoint
            var response = await batchEndpointClient.PostAsync(_azureMLBatchClientOptions.EndpointUri, jsonContent);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var jobId = jsonResponse.GetProperty("name").GetString();

            return jobId;
        }


        private static async Task<HttpClient> InitializeHttpClient() 
        {
            // Authenticate and get an access token
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                TenantId = "TenantId"
            });

            var tokenRequestContext = new Azure.Core.TokenRequestContext(["https://ml.azure.com"]);
            var token = await credential.GetTokenAsync(tokenRequestContext);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            return httpClient;
        }

        public async Task<string> PingJobStatus(string jobId) 
        {
            using var batchEndpointClient = await InitializeHttpClient();

            // URL for monitoring the batch job status
            var statusUrl = _azureMLBatchClientOptions.EndpointUri + $"/{jobId}";
            // Monitor the job status
            var statusResponse = await batchEndpointClient.GetAsync(statusUrl);
            statusResponse.EnsureSuccessStatusCode();

            var statusBody = await statusResponse.Content.ReadAsStringAsync();
            var statusJsonResponse = JsonSerializer.Deserialize<JsonElement>(statusBody);
            var jobStatus = statusJsonResponse.GetProperty("properties").GetProperty("status").GetString();

            var result = "Pending";
            if (jobStatus == "Completed" || jobStatus == "Failed")
            {
                result = jobStatus;
            }

            return result;
        }
    }
}
