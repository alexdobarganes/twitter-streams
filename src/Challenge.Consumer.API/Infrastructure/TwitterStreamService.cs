using Challenge.Consumer.API.Events;
using Challenge.Consumer.API.Infrastructure;

public class TwitterStreamService: IEventStream
{
    private readonly TwitterStreamClient _twitterStreamClient;
    private readonly IEventDispatcher _eventDispatcher;

    public TwitterStreamService(TwitterStreamClient twitterStreamClient, IEventDispatcher eventDispatcher)
    {
        _twitterStreamClient = twitterStreamClient;
        _eventDispatcher = eventDispatcher;
    }

    public IEventStream Stream<T>() where T : class, IEvent
    {
        _ = _twitterStreamClient.StartStreamAsync<T>(async t => await _eventDispatcher.PublishAsync(t));
        return this;
    }
}