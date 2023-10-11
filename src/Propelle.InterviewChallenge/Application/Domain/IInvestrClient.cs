namespace Propelle.InterviewChallenge.Application.Domain
{
    public interface IInvestrClient
    {
        IEnumerable<(Guid UserId, decimal Amount)> SubmittedDeposits { get; }

        Task SubmitDeposit(Guid userId, decimal amount);
    }
}
