namespace Propelle.InterviewChallenge.Application.Domain
{
    public class InvestrClient : IInvestrClient
    {
        private readonly List<(Guid UserId, decimal Amount)> _submittedDeposits;

        public IEnumerable<(Guid UserId, decimal Amount)> SubmittedDeposits => _submittedDeposits;

        public InvestrClient()
        {
            _submittedDeposits = new List<(Guid UserId, decimal Amount)>();
        }

        public Task SubmitDeposit(Guid userId, decimal amount)
        {
            PointOfFailure.SimulatePotentialFailure();

            _submittedDeposits.Add((userId, amount));

            return Task.CompletedTask;
        }
    }
}
