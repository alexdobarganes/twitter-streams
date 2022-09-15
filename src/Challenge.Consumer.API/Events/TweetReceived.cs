using Challenge.Consumer.API.Events;

namespace Challenge.Consumer.API.DTO;

public record TweetReceived: IEvent
{
    public string Id { get; set; }
    public string Text { get; set; }
    //public PublicMetric PublicMetrics { get; set; }
    public string Source { get; set; }
}

public record PublicMetric {
    public int RetweetCount { get; set; }
    public int QuoteCount { get; set; }
    public int LikeCount { get; set; }
}