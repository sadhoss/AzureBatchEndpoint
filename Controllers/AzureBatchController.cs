using Microsoft.AspNetCore.Mvc;

namespace AzureBatchEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureBatchController(ILogger<AzureBatchController> logger) : ControllerBase
    {
        private readonly ILogger<AzureBatchController> _logger = logger;

        [HttpGet(Name = "RunAzureBatchEndpointPrediction")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
