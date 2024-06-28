using Microsoft.AspNetCore.Mvc;

namespace AzureBatchEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureBatchController(ILogger<AzureBatchController> logger, AzureMLBatchService azureMLBatchService) : ControllerBase
    {
        private readonly ILogger<AzureBatchController> _logger = logger;
        private readonly AzureMLBatchService _azureMLBatchService = azureMLBatchService;

        [HttpPost(Name = "RunAzureBatchEndpointPrediction")]
        public async Task<ActionResult<string?>> Post()
        {
            return await _azureMLBatchService.RunBatchEndpoint();
        }

        [HttpGet(Name = "GetAzureBatchEndpointPrediction")]
        public async Task<ActionResult<ModelPrediction>> Get(string jobId)
        {
            return await _azureMLBatchService.GetBatchEndpointPrediction(jobId);
        }
    }
}
