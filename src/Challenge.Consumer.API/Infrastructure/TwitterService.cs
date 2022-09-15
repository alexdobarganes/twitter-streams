//using Challenge.Consumer.API.DTO;
//using Challenge.Consumer.API.Infrastructure;

//internal class TwitterService : BackgroundService
//{
//    private readonly TwitterStreamClient _twitterStreamClient;
//    private readonly IEventDispatcher _eventDispatcher;

//    public TwitterService(TwitterStreamClient twitterStreamClient, IEventDispatcher eventDispatcher)
//    {
//        _twitterStreamClient = twitterStreamClient;
//        _eventDispatcher = eventDispatcher;
//    }
//    protected override Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        return _twitterStreamClient.StartStreamAsync<TweetReceived>(async t => await _eventDispatcher.PublishAsync(t), stoppingToken);
//    }
//}
