using App.Metrics;
using App.Metrics.Formatters.Json;
using App.Metrics.Formatters.Json.Extensions;
using Challenge.Consumer.API.Collectors;

namespace Challenge.Consumer.API.Metrics;

public class TwitterMetricsService : ITwitterMetrics
{
    private readonly IHashtagCollector _hashtagCollector;
    private readonly IMetrics _metrics;

    public TwitterMetricsService(IHashtagCollector hashtagCollector, IMetrics metrics)
    {
        _hashtagCollector = hashtagCollector;
        _metrics = metrics;
    }

    public MetricData GetMetricsData()
    {
        return _metrics.Snapshot.Get().ToMetric();
    }

    public IReadOnlyList<HashtagMetric> GetTopTenHashtags()
    {
        return _hashtagCollector.GetCollectedHashtagsWithEntries()
            .GroupBy(x => x)
            .Select(m => new HashtagMetric { Hashtag = m.Key.Key, Entries = m.Key.Value })
            .OrderByDescending(x => x.Entries).Take(10).ToList().AsReadOnly();
    }
}