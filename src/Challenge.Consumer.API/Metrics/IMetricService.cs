namespace Challenge.Consumer.API.Metrics
{

    public interface IHashtagMetrics
    {
        IReadOnlyList<HashtagMetric> GetTopTenHashtags();
    }
}