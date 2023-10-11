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
        public async Task MakeDeposit_XTimesSuccessfully_AlwaysProcessesXDeposits(int iterations)
        {
            var client = _factory.CreateClient();

            for (var i = 0; i < iterations; i++)
            {
                // Simulate a customer retrying a deposit if something goes wrong
                await TryUntilSuccessful(
                    () => client.POSTAsync<MakeDeposit.Endpoint, MakeDeposit.Request, MakeDeposit.Response>(GenerateMakeDepositRequest()),
                    x => x.Response.IsSuccessStatusCode);
            }

            // Assertions
            using var scope = _factory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetService<PaymentsContext>();
            var investrClient = scope.ServiceProvider.GetService<IInvestrClient>();

            var deposits = await context.Deposits.ToListAsync();
            var sentDeposits = investrClient.SubmittedDeposits.ToList();

            Assert.Equal(iterations, deposits.Count);
            Assert.Equal(iterations, sentDeposits.Count);

            foreach (var deposit in deposits)
            {
                Assert.Contains((deposit.UserId, deposit.Amount), sentDeposits);
            }
        }

        [Theory]
        [InlineData(100)]
        public async Task MakeDeposit_XTimesSuccessfully_DoesntAttemptSubmissionsDuringRequest(int iterations)
        {
            var client = _factory.CreateClient();

            for (var i = 0; i < iterations; i++)
            {
                // Simulate a customer retrying a deposit if something goes wrong
                await TryUntilSuccessful(
                    () => client.POSTAsync<MakeDeposit.Endpoint, MakeDeposit.Request, MakeDeposit.Response>(GenerateMakeDepositRequest()),
                    x => x.Response.IsSuccessStatusCode);
            }

            using var scope = _factory.Services.CreateScope();
            var investrClient = scope.ServiceProvider.GetService<IInvestrClient>();
        }

        private static async Task<T> TryUntilSuccessful<T>(
            Func<Task<T>> @delegate,
            Func<T, bool> successCondition = null)
        {
            bool isSuccess = false;
            while (!isSuccess)
            {
                try
                {
                    var result = await @delegate();
                    if (successCondition == default || successCondition(result))
                    {
                        return result;
                    }
                }
                catch
                {
                }
            }

            return default;
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