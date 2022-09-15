using App.Metrics;
using Challenge.Consumer.API.Metrics;

namespace Challenge.Consumer.API.Events.Handlers
{
    public class CaptureMetricsEventHandler : IEventHandler<TweetReceived>
    {
        private readonly IMetrics _metric;

        public CaptureMetricsEventHandler(IMetrics metrics)
        {
            _metric = metrics;
        }
        public Task HandleAsync(TweetReceived @event, CancellationToken cancellationToken = default)
        {
            _metric.Measure.Counter.Increment(MetricsRegistry.ReceivedTweetsCounter);
            return Task.CompletedTask;
        }
    }
}
