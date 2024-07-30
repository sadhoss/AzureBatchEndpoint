using AzureBatchEndpoint.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureBatchEndpoint.Controllers
{
    [ApiController]
    [Route("api/AzureBatchEndpoint/[action]")]

    public class AzureBatchController(ILogger<AzureBatchController> logger, MLService azureMLBatchService) : ControllerBase
    {
        private readonly ILogger<AzureBatchController> _logger = logger;
        private readonly MLService _azureMLBatchService = azureMLBatchService;

        [HttpPost]
        public async Task<ActionResult<PreModelPrediction>> RunPrediction(UnpraisedDiamond unpraisedDiamond)
        {
            return await _azureMLBatchService.Predict(unpraisedDiamond);
        }

        [HttpGet]
        public async Task<ActionResult<PostModelPrediction>> GetPrediction(string jobId, string filePath)
        {
            return await _azureMLBatchService.GetPrediction(jobId, filePath);
        }
    }
}
