namespace Propelle.InterviewChallenge.Application
{
    [Serializable]
    public class PointOfFailureException : Exception
    {
        public PointOfFailureException() : base("Oh no! Something didn't work") { }
    }
}
