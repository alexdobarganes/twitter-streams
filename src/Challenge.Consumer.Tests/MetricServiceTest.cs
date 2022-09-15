using App.Metrics;
using App.Metrics.Counter;
using Challenge.Consumer.API.Collectors;
using Challenge.Consumer.API.Metrics;

namespace Challenge.Consumer.Tests;

public class MetricServiceTest
{
    private readonly HashtagCollector _collector;
    private readonly TwitterMetricsService _sut;

    public MetricServiceTest()
    {
        new MetricsBuilder().Build();
        _collector = new HashtagCollector(Metrics.Instance);
        _sut = new TwitterMetricsService(_collector, Metrics.Instance);
    }

    [Fact]
    public void ShouldTrackTheTop10Tags()
    {
        var tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#1")));
        for (int i = 0; i < 9; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#2")));
        for (int i = 0; i < 8; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#3")));
        for (int i = 0; i < 7; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#4")));
        for (int i = 0; i < 6; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#5")));
        for (int i = 0; i < 5; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#6")));
        for (int i = 0; i < 4; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#7")));
        for (int i = 0; i < 3; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#8")));
        for (int i = 0; i < 2; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#9")));
        for (int i = 0; i < 2; i++)
            tasks.Add(Task.Run(() => _collector.Collect("#10")));

        Task.WaitAll(tasks.ToArray());
        var hashtags = _sut.GetTopTenHashtags().ToArray();
        for (int i = 0; i < hashtags.Length - 1; i++)
        {
            Assert.True(hashtags[i].Entries >= hashtags[i + 1].Entries);
        }
    }

    [Fact]
    public void ShouldTotalNumberOfTweetCapturedConcurrently()
    {
        var tasks = new List<Task>(10);
        for (int i = 0; i < 10; i++)
            tasks.Add(Task.Run(() => Metrics.Instance.Measure.Counter.Increment(MetricsRegistry.ReceivedTweetsCounter)));

        Task.WaitAll(tasks.ToArray());
        Assert.Equal(10, Metrics.Instance.Snapshot.GetCounterValue(MetricsRegistry.ReceivedTweetsCounter.Context, MetricsRegistry.ReceivedTweetsCounter.Name).Count);
    }
}