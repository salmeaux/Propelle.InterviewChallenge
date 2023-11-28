using Propelle.InterviewChallenge.FailureHandling;

namespace Propelle.InterviewChallenge.Tests
{
    public class InfiniteRetryOnErrorStrategyTests
    {
        public class TaskToPerform
        {
            private int _count = 0;
            public async Task DoAsync(int? throwUntil = default(int?))
            {
                await Task.Run(() => 
                {
                    _count++;
                    if (throwUntil != null && throwUntil >= _count) {
                        throw new Exception();
                    } 
                });
            }
        }
        [Fact]
        public async Task GivenErroringTaskShouldWaitUntilSuccessfully()
        {
            var taskToPerform = new TaskToPerform();
            var systemUnderTest = new InfiniteRetryOnErrorStrategy();
            var task = systemUnderTest.ImplementAsync(() => taskToPerform.DoAsync(5));
            await task;
            Assert.True(task.IsCompletedSuccessfully);
        }
        [Fact]
        public async Task GivenNonErroringTaskShouldCompleteSuccessfully()
        {
            var taskToPerform = new TaskToPerform();
            var systemUnderTest = new InfiniteRetryOnErrorStrategy();
            var task = systemUnderTest.ImplementAsync(() => taskToPerform.DoAsync());
            await task;
            Assert.True(task.IsCompletedSuccessfully);
        }
    }
}