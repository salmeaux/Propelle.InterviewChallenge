using Propelle.InterviewChallenge.EventHandling;

namespace Propelle.InterviewChallenge.Application.EventBus
{
    public class SimpleEventBus : IEventBus
    {
        private readonly InMemoryEventExchange _exchange;

        public SimpleEventBus(InMemoryEventExchange exchange)
        {
            _exchange = exchange;
        }

        public async Task Publish<TEvent>(TEvent @event)
            where TEvent : class
        {
            await _exchange.Publish(@event);
        }
    }
}
