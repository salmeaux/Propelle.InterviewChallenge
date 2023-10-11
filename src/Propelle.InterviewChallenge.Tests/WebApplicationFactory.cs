using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Propelle.InterviewChallenge.Application.Domain;
using Propelle.InterviewChallenge.Tests.Metrics;

namespace Propelle.InterviewChallenge.Tests
{
    public class WebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<InvestrClientMetricsStore>();

                services.RemoveAll(typeof(IInvestrClient));
                services.AddSingleton<InvestrClient>();
                services.AddScoped<InvestrClientMetricsDecorator>();
                services.AddScoped<IInvestrClient, InvestrClientMetricsDecorator>();
            });
            base.ConfigureWebHost(builder);
        }
    }
}
