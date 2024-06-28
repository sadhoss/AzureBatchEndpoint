using Microsoft.AspNetCore.Mvc;

namespace AzureBatchEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureBatchController(ILogger<AzureBatchController> logger) : ControllerBase
    {
        private readonly ILogger<AzureBatchController> _logger = logger;

        [HttpGet(Name = "RunAzureBatchEndpointPrediction")]
        public IEnumerable<ModelPrediction> Get()
        {
            return [new ModelPrediction
            {
                ModelVersion = "404",
                ModelName = "Not Found",
                Prediction = "",
                ModelInputParameterOne = "One",
                ModelInputParameterTwo = "Two"
            }];
        }
    }
}
