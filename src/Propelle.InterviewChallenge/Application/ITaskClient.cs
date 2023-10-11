namespace Propelle.InterviewChallenge.Application
{
    public interface ITaskClient
    {
        int ProcessedTasksCount { get; }

        Task Enqueue(Func<Task> task);
    }
}
