using Propelle.InterviewChallenge.Application.Domain;
using Propelle.InterviewChallenge.Application.Domain.Events;
using Propelle.InterviewChallenge.EventHandling;

namespace Propelle.InterviewChallenge.Application.EventHandlers
{
    public class SubmitDeposit : IEventHandler<DepositMade>
    {
        private readonly PaymentsContext _context;
        private readonly IInvestrClient _investrClient;

        public SubmitDeposit(PaymentsContext context, IInvestrClient investrClient)
        {
            _context = context;
            _investrClient = investrClient;
        }

        public async Task Handle(DepositMade @event)
        {
            var deposit = await _context.Deposits.FindAsync(@event.Id);

            await _investrClient.SubmitDeposit(deposit.UserId, deposit.Amount);
        }
    }
}
