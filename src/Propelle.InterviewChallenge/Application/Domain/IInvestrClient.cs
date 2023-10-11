namespace Propelle.InterviewChallenge.Application.Domain
{
    public interface IInvestrClient
    {
        IEnumerable<(Guid UserId, decimal Amount)> SentDeposits { get; }

        Task MakeDeposit(Guid userId, decimal amount);
    }
}
