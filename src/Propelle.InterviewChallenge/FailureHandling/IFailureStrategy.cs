namespace Propelle.InterviewChallenge.FailureHandling
{
    public interface IFailureStrategy
    {
        public Task ImplementAsync(Func<Task> @delegate);
    }
}