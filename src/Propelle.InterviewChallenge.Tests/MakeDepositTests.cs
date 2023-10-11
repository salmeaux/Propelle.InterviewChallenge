using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Propelle.InterviewChallenge.Application;
using Propelle.InterviewChallenge.Application.Domain;
using Propelle.InterviewChallenge.Endpoints;

namespace Propelle.InterviewChallenge.Tests
{
    public class MakeDepositTests : IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _factory;

        public MakeDepositTests(WebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(100)]
        public async Task MakeDeposit_XTimesSuccessfully_AlwaysProcessesXDepositsWithInvestr(int iterations)
        {
            var client = _factory.CreateClient();

            for (var i = 0; i < iterations; i++)
            {
                // Simulate a customer retrying a deposit if something goes wrong
                bool isSuccess = false;
                while (!isSuccess)
                {
                    try
                    {
                        var (result, _) = await client.POSTAsync<MakeDeposit.Endpoint, MakeDeposit.Request, MakeDeposit.Response>(GenerateMakeDepositRequest());
                        isSuccess = result.IsSuccessStatusCode;
                    }
                    catch
                    {
                    }
                }
            }

            // Assertions
            using var scope = _factory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetService<PaymentsContext>();
            var investrClient = scope.ServiceProvider.GetService<IInvestrClient>();

            Assert.Equal(iterations, await context.Deposits.CountAsync());
            Assert.Equal(iterations, investrClient.SentDeposits.Count());
        }

        private static MakeDeposit.Request GenerateMakeDepositRequest()
        {
            return new MakeDeposit.Request
            {
                UserId = Guid.NewGuid(),
                Amount = new Random().Next(100, 1000)
            };
        }
    }
}