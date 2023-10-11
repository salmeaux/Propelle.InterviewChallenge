using Propelle.InterviewChallenge.Application.Domain;

namespace Propelle.InterviewChallenge.Tests.Metrics
{
    public class InvestrClientMetricsDecorator : IInvestrClient
    {
        private readonly InvestrClient _inner;
        private readonly InvestrClientMetricsStore _metricsStore;

        private int _makeDepositAttemptsThisRequest = 0;

        public InvestrClientMetricsDecorator(
            InvestrClient inner,
            InvestrClientMetricsStore metricsStore)
        {
            _inner = inner;
            _metricsStore = metricsStore;
        }

        public IEnumerable<(Guid UserId, decimal Amount)> SubmittedDeposits => _inner.SubmittedDeposits;

        public async Task SubmitDeposit(Guid userId, decimal amount)
        {
            _makeDepositAttemptsThisRequest++;

            await _inner.SubmitDeposit(userId, amount);

            _metricsStore.RecordAttemptsPerRequest((userId, amount), _makeDepositAttemptsThisRequest);
        }
    }
}
