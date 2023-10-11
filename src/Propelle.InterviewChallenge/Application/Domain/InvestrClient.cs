namespace Propelle.InterviewChallenge.Application.Domain
{
    public class InvestrClient : IInvestrClient
    {
        private readonly List<(Guid UserId, decimal Amount)> _sentDeposits;

        public IEnumerable<(Guid UserId, decimal Amount)> SentDeposits => _sentDeposits;

        public InvestrClient()
        {
            _sentDeposits = new List<(Guid UserId, decimal Amount)>();
        }

        public Task MakeDeposit(Guid userId, decimal amount)
        {
            PointOfFailure.SimulatePotentialFailure();

            _sentDeposits.Add((userId, amount));

            return Task.CompletedTask;
        }
    }
}
