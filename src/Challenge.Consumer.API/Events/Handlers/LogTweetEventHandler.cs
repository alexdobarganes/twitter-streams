//using Challenge.Consumer.API.DTO;

//namespace Challenge.Consumer.API.Events
//{
//    public class LogTweetEventHandler : IEventHandler<TweetReceived>
//    {
//        private readonly ILogger<LogTweetEventHandler> _logger;

//        public LogTweetEventHandler(ILogger<LogTweetEventHandler> logger)
//        {
//            _logger = logger;
//        }
//        public Task HandleAsync(TweetReceived @event, CancellationToken cancellationToken = default)
//        {
//            _logger.LogInformation("New tweet received {tweet}", @event.Text);
//            return Task.CompletedTask;
//        }
//    }
//}
