using FastEndpoints;
using Propelle.InterviewChallenge.FailureHandling;
using Propelle.InterviewChallenge.Application;
using Propelle.InterviewChallenge.Application.Domain;
using Propelle.InterviewChallenge.Application.Domain.Events;

namespace Propelle.InterviewChallenge.Endpoints
{
    public static class MakeDeposit
    {
        public class Request
        {
            public Guid UserId { get; set; }

            public decimal Amount { get; set; }
        }

        public class Response
        {
            public Guid DepositId { get; set; }
        }

        public class Endpoint : Endpoint<Request, Response>
        {
            private readonly PaymentsContext _paymentsContext;
            private readonly Application.EventBus.IEventBus _eventBus;

            public Endpoint(
                PaymentsContext paymentsContext,
                Application.EventBus.IEventBus eventBus)
            {
                _paymentsContext = paymentsContext;
                _eventBus = eventBus;
            }

            public override void Configure()
            {
                Post("/api/deposits/{UserId}");
            }

            public override async Task HandleAsync(Request req, CancellationToken ct)
            {
                var deposit = new Deposit(req.UserId, req.Amount);
                _paymentsContext.Deposits.Add(deposit);

                var retryStrategy = new InfiniteRetryOnErrorStrategy();

                await FailureHandler.HandleAsync(
                    () => _paymentsContext.SaveChangesAsync(ct), 
                    retryStrategy);

                await FailureHandler.HandleAsync(
                    () => _eventBus.Publish(new DepositMade
                        {
                            Id = deposit.Id
                        }), 
                    retryStrategy);

                await FailureHandler.HandleAsync(
                    () => SendAsync(new Response { DepositId = deposit.Id }, 201, ct), 
                    retryStrategy);
            }
        }
    }
}
