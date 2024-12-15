using Microsoft.AspNetCore.Mvc;

namespace AradaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : Controller
    {
        private readonly HttpClient _httpClient;
        public ExchangeRateController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet("usd-try")]
        public async Task<IActionResult> GetUsdToTryRate()
        {
            var url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return Content(response, "application/xml");
            }
            catch
            {
                return StatusCode(500, "Error fetching exchange rates from TCMB.");
            }
        }
    }
}
