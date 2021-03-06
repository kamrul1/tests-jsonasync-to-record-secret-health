using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherClient weatherClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherClient weatherClient)
        {
            _logger = logger;
            this.weatherClient = weatherClient;
        }

        [HttpGet("{country}/{city}")]
        public async Task<WeatherForecast> GetAsync(string country, string city)
        {
            var forcast = await weatherClient.GetCurrentCityCountryForecastAsync(city, country);
            

            return new WeatherForecast
            {
                Summary = forcast.data[0].weather.description,
                TemperatureC = (int)forcast.data[0].temp,
                Date = DateTime.Parse(forcast.data[0].ob_time)

            };

        }
    }
}
