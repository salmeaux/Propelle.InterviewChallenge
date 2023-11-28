namespace Propelle.InterviewChallenge.FailureHandling
{
    public static class FailureHandler {
        public static async Task HandleAsync(
                Func<Task> @delegate,
                IFailureStrategy strategy)
            {
                await strategy.ImplementAsync(@delegate);
            }
    }
}