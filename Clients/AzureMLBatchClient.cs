using Azure.Identity;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AzureBatchEndpoint.Clients
{
    public record AzureMLBatchClientOptions
    {
        public string TenantId { get; set; } = "063afd9e-5fcb-48d2-a769-ca31b0f5b443";
        public string SubscriptionId { get; set; } = "0e834d8d-36cc-4ec6-b444-a014c2fcae53";
        public string ResourceGroupName { get; set; } = "SadeghHosseinpoorTestEnv";
        public string WorkspaceName { get; set; } = "testlabeling";
        public string Datastore { get; set; } = "workspaceblobstore";
        public string EndpointName { get; set; } = "testlabeling-zewly";
        public string ApiVersion { get; set; } = "2023-04-01";
        public string EndpointUri { get; set; } = "https://testlabeling-zewly.westeurope.inference.ml.azure.com/jobs";
    }

    public class AzureMLBatchClient()
    {
        private readonly AzureMLBatchClientOptions _azureMLBatchClientOptions = new();

        public async Task<string?> InvokeBatchEndpoint(string filepath)
        {
            using var batchEndpointClient = await InitializeHttpClient();

            var uriFilepath = $"azureml://subscriptions/{_azureMLBatchClientOptions.SubscriptionId}/resourcegroups/{_azureMLBatchClientOptions.ResourceGroupName}/workspaces/{_azureMLBatchClientOptions.WorkspaceName}/datastores/{_azureMLBatchClientOptions.Datastore}/paths/{filepath}";

            // Prepare the request body
            var requestBody = new
            {
                properties = new
                {
                    InputData = new
                    {
                        DiamondPricing = new
                        {
                            JobInputType = "UriFile",
                            Uri = uriFilepath + ".csv"
                        }
                    },
                    OutputData = new
                    {
                        score = new
                        {
                            JobOutputType = "UriFile",
                            Uri = uriFilepath + "_predictions.csv"
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

        private async Task<HttpClient> InitializeHttpClient()
        {
            // Authenticate and get an access token
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                TenantId = _azureMLBatchClientOptions.TenantId,
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
