namespace Propelle.InterviewChallenge.Application
{
    public static class PointOfFailure
    {
        public static Random _random;

        static PointOfFailure()
        {
            _random = new Random();
        }

        public static void SimulatePotentialFailure()
        {
            var val = _random.Next(0, 6);

            if (val == 5)
            {
                throw new TransientException();
            }
        }
    }
}
