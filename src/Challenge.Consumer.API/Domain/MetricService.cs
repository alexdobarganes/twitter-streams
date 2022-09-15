using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Challenge.Consumer.API.Domain;

public class MetricService : IMetricService
{
    private long _entriesReceived;
    private readonly long _initiated;
    private readonly Regex _regex;
    private readonly ConcurrentDictionary<string, long> _hashtags = new();

    public MetricService()
    {
        _initiated = DateTime.Now.Ticks;
        _regex = new Regex(@"#\w+");
    }

    public void Capture(string text)
    {
        Interlocked.Increment(ref _entriesReceived);

        foreach (var match in _regex.Matches(text))
        {
            _hashtags.AddOrUpdate(match?.ToString(), 1, (key, qty) => qty + 1);
        }
    }

    public int GetEntriesPerSecond()
    {
        var result = _entriesReceived;
        var timeInSeconds = (DateTime.Now.Ticks - _initiated) / TimeSpan.TicksPerSecond;
        if (timeInSeconds < 1)
            result = _entriesReceived;
        else
            result /= timeInSeconds;
        return Convert.ToInt32(result);
    }

    public long GetTotalNumberOfEntriesReceived() => _entriesReceived;

    /// <summary>
    /// Obtains the top 10 entries based only on the number of occurences.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<HashtagMetric> GetTopTenHashtags()
        => _hashtags.OrderByDescending(x => x.Value).Take(10).Select(y => new HashtagMetric(y.Key, y.Value)).ToList().AsReadOnly();

    public long GetTotalNumberHashtagsFound() => _hashtags.Count;
}