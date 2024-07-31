using AzureBatchEndpoint.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureBatchEndpoint.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class AzureBatchController(ILogger<AzureBatchController> logger, MLService azureMLBatchService) : ControllerBase
    {
        private readonly ILogger<AzureBatchController> _logger = logger;
        private readonly MLService _azureMLBatchService = azureMLBatchService;

        /// <summary>
        /// Use the RunPrediction with appropriate diamond attributes (4Cs) to get an apraisal. Overview of accepted attribute values https://4cs.gia.edu/en-us/blog/gia-diamond-grading-scales/
        /// </summary>
        /// <remarks>
        /// Dataset contains following values, attributes are limited accordingly.
        /// Carat
        ///     - [0.2 -> 5.01]
        /// Clarity
        ///     - [I1, IF, SI1, SI2, VS1, VS2, VVS1, VVS2]
        /// Colour
        ///     - [D -> j]
        /// Cut
        ///     - [Fair, Good, Ideal, Premium, Very Good]
        ///     
        /// 
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///        Carat : 0.5
        ///        Clarity : VVS1
        ///        Colour : H
        ///        Cut : Fair
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<PreModelPrediction>> RunPrediction(UnpraisedDiamond unpraisedDiamond)
        {
            return await _azureMLBatchService.Predict(unpraisedDiamond);
        }
        /// <summary>
        /// Use the GetPrediction with the metadata information (jobId and filepath) returned from the RunPrediction, to get the price estimate.
        /// Running GetPrediction with the metadata info will also indicate progress with job status (Pending, Failed, Completed).
        /// Wait time is about 15minutes. 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PostModelPrediction>> GetPrediction(string jobId, string filePath)
        {
            return await _azureMLBatchService.GetPrediction(jobId, filePath);
        }
    }
}
