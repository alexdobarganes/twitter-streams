using App.Metrics;
using App.Metrics.Counter;

namespace Challenge.Consumer.API.Metrics;

public static class MetricsRegistry
{
    public static CounterOptions ReceivedTweetsCounter => new CounterOptions
    {
        Name = "Received Tweets",
        Context = "StreamsApi",
        MeasurementUnit = Unit.Calls
    };

    public static CounterOptions NewHashtagsFound => new CounterOptions
    {
        Name = "New Hashtags Founds",
        Context = "StreamsApi",
        MeasurementUnit = Unit.Calls
    };
}
