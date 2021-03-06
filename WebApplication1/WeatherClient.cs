using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class WeatherClient
    {
        private readonly HttpClient httpClient;
        private readonly ServiceSettings serviceSettings;

        public WeatherClient(IOptions<ServiceSettings> options, HttpClient client)
        {
            serviceSettings = options.Value;
            client.BaseAddress = new Uri(serviceSettings.WeatherBitHost);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient = client;

        }

        public record Data(string ob_time, decimal temp, Weather weather);
        public record Weather(string description);
        public record Forecast(Data[] data);


        public Task<Forecast> GetCurrentCityCountryForecastAsync(string city, string country)
        {

            var urlEncodeCountry = WebUtility.UrlEncode(country);
            var host = $"/v2.0/current?city={city}&country={urlEncodeCountry}&key={serviceSettings.ApiKey}";
            //var test = await httpClient.GetStringAsync(host); 
            //var forecast = JsonSerializer.Deserialize<Forecast>(test);
           

            return httpClient.GetFromJsonAsync<Forecast>(host);

        }

    }
}
