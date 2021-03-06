using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class ExternalEndPointHealthCheck : IHealthCheck
    {
        private readonly ServiceSettings serviceSettings;
        private readonly ILogger<ExternalEndPointHealthCheck> logger;

        public ExternalEndPointHealthCheck(IOptions<ServiceSettings> options, ILogger<ExternalEndPointHealthCheck> log)
        {
            serviceSettings = options.Value;
            logger = log;
        }


        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            
            var hostName = Regex.Replace(serviceSettings.WeatherBitHost, "^https://", "");
            logger.LogInformation($"Pinging: {hostName}");

            Ping ping = new();
            var reply = await ping.SendPingAsync(hostName);
            
            return reply.Status != IPStatus.Success ? 
                        HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
        }
    }
}
