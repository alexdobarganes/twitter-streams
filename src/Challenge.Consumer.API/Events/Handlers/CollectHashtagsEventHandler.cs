using Challenge.Consumer.API.Collectors;

namespace Challenge.Consumer.API.Events.Handlers;

internal class CollectHashtagsEventHandler : IEventHandler<TweetReceived>
{
    private readonly IHashtagCollector _hashtagCollector;

    public CollectHashtagsEventHandler(IHashtagCollector hashtagCollector)
    {
        _hashtagCollector = hashtagCollector;
    }
    public Task HandleAsync(TweetReceived @event, CancellationToken cancellationToken = default)
    {
        _hashtagCollector.Collect(@event.Text);
        return Task.CompletedTask;
    }
}
