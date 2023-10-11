using System.Collections.Concurrent;

namespace Propelle.InterviewChallenge.Application
{
    public class TaskClient : ITaskClient
    {
        private int _processed;

        public int ProcessedTasksCount => _processed;

        public async Task Enqueue(Func<Task> task)
        {
            try
            {
                PointOfFailure.SimulatePotentialFailure();
                await task();
                _processed++;
            }
            catch
            {
            }
        }
    }
}
