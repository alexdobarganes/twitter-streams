using Challenge.Consumer.API.Domain;
using Challenge.Consumer.API.DTO;

namespace Challenge.Consumer.API.Events.Handlers
{
    public class CaptureMetricsEventHandler : IEventHandler<TweetReceived>
    {
        private readonly IMetricService _metricService;

        public CaptureMetricsEventHandler(IMetricService metricService)
        {
            _metricService = metricService;
        }
        public Task HandleAsync(TweetReceived @event, CancellationToken cancellationToken = default)
        {
            _metricService.Capture(@event.Text);
            return Task.CompletedTask;
        }
    }
}
