using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propelle.InterviewChallenge.Tests.Metrics
{
    public class InvestrClientMetricsStore
    {
        private readonly Dictionary<string, int> _data;

        public InvestrClientMetricsStore()
        {
            _data = new Dictionary<string, int>();
        }

        public void RecordAttemptsPerRequest((Guid UserId, decimal Amount) deposit, int attempts)
        {
            _data.Add(Hash(deposit), attempts);
        }

        public int? GetAttempts((Guid UserId, decimal Amount) deposit)
        {
            if (_data.TryGetValue(Hash(deposit), out int attempts))
            {
                return attempts;
            }

            return default;
        }

        private static string Hash((Guid UserId, decimal Amount) key)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{key.UserId}:{key.Amount}"));
        }
    }
}
