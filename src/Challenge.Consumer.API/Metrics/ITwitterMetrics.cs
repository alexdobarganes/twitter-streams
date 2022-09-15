using App.Metrics.Formatters.Json;

namespace Challenge.Consumer.API.Metrics
{
    internal interface ITwitterMetrics
    {
        public MetricData GetMetricsData();
        IReadOnlyList<HashtagMetric> GetTopTenHashtags();
    }
}