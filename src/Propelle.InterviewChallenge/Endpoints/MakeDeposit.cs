using FastEndpoints;
using Propelle.InterviewChallenge.Application;
using Propelle.InterviewChallenge.Application.Domain;

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
            private readonly IInvestrClient _investrClient;

            public Endpoint(
                PaymentsContext paymentsContext,
                IInvestrClient investrClient)
            {
                _paymentsContext = paymentsContext;
                _investrClient = investrClient;
            }

            public override void Configure()
            {
                Post("/api/deposits/{UserId}");
            }

            public override async Task HandleAsync(Request req, CancellationToken ct)
            {
                var deposit = new Deposit(req.UserId, req.Amount);
                _paymentsContext.Deposits.Add(deposit);

                await _paymentsContext.SaveChangesAsync(ct);
                await _investrClient.MakeDeposit(deposit.UserId, deposit.Amount);

                await SendAsync(new Response { DepositId = deposit.Id }, 201, ct);
            }
        }
    }
}
