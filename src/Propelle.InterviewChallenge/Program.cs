using FastEndpoints;
using Propelle.InterviewChallenge.Application;
using Propelle.InterviewChallenge.Application.Domain;

namespace Propelle.InterviewChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<PaymentsContext>();
            builder.Services.AddSingleton<IInvestrClient, InvestrClient>();
            builder.Services.AddSingleton<ITaskClient, TaskClient>();
            builder.Services.AddFastEndpoints();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseFastEndpoints(x =>
            {
                /* Allow anonymous access globally for the purposes of this exercise */
                x.Endpoints.Configurator = epd => epd.AllowAnonymous();
            });
            app.Run();
        }
    }
}