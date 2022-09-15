using App.Metrics;
using Challenge.Consumer.API.Metrics;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Challenge.Consumer.API.Collectors;

/// <summary>
/// Collects found hashtags in the tweet.
/// </summary>
public class HashtagCollector : IHashtagCollector
{
    //TODO: would be Intresting to explore more the AppMetrics library to track this metric
    private readonly ConcurrentDictionary<string, long> _hashtags = new();
    //private readonly ConcurrentBag<string> _colletedHashtags = new();
    private readonly IMetrics _metrics;
    private Regex _regex;

    public HashtagCollector(IMetrics metrics)
    {
        _regex = new Regex(@"#\w+");
        _metrics = metrics;
    }
    public void Collect(string text)
    {
        foreach (var match in _regex.Matches(text))
        {
            //_colletedHashtags.Add(match?.ToString());
            //if (!_colletedHashtags.Contains(match.ToString()))
            //    _metrics.Measure.Counter.Increment(MetricsRegistry.NewHashtagsFound);

            //this is an optimization to exclude duplicates
            _hashtags.AddOrUpdate(match?.ToString(), ht =>
            {
                _metrics.Measure.Counter.Increment(MetricsRegistry.NewHashtagsFound);
                return 1;
            }, (key, qty) => qty + 1);
        }
    }

    public IReadOnlyList<string> GetCollectedHashtags() => _hashtags.Select(x => x.Key).ToList().AsReadOnly();

    public IReadOnlyDictionary<string, long> GetCollectedHashtagsWithEntries() => _hashtags.ToImmutableDictionary();
}
