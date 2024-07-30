using AzureBatchEndpoint.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureBatchEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureBatchController(ILogger<AzureBatchController> logger, MLService azureMLBatchService) : ControllerBase
    {
        private readonly ILogger<AzureBatchController> _logger = logger;
        private readonly MLService _azureMLBatchService = azureMLBatchService;

        [HttpPost(Name = "RunPrediction")]
        public async Task<ActionResult<ModelPrediction>> Post(Diamond diamond)
        {
            return await _azureMLBatchService.Predict(diamond);
        }

        [HttpGet(Name = "GetPrediction")]
        public async Task<ActionResult<ModelPrediction>> Get(string jobId, string filePath)
        {
            return await _azureMLBatchService.GetPrediction(jobId, filePath);
        }
    }
}
