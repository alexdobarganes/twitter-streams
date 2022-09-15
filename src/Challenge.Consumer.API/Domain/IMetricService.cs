namespace Challenge.Consumer.API.Domain
{
    public interface IMetricService
    {
        long GetTotalNumberOfEntriesReceived();
        long GetTotalNumberHashtagsFound();
        void Capture(string text);
        int GetEntriesPerSecond();
        IReadOnlyList<HashtagMetric> GetTopTenHashtags();
    }
}