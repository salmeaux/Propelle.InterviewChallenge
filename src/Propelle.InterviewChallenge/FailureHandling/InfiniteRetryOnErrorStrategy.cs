namespace Propelle.InterviewChallenge.FailureHandling
{
    public class InfiniteRetryOnErrorStrategy : IFailureStrategy
    {
        public async Task ImplementAsync(Func<Task> @delegate)
        {
            var isSuccess = false;
            while(!isSuccess) 
            {
                try
                {
                    var task = @delegate();
                    await task;
                    isSuccess = task.IsCompletedSuccessfully;
                }
                catch 
                {
                    continue;
                }
            }
        }
    }
}