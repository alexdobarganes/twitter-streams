using Challenge.Consumer.API.Domain;

namespace Challenge.Consumer.Tests;

public class MetricServiceTest {
    [Fact]
    public void ShouldTrackTheTop10Tags()
    {
        var _sut = new MetricService();
        var tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#1")));
        for (int i = 0; i < 9; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#2")));
        for (int i = 0; i < 8; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#3")));
        for (int i = 0; i < 7; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#4")));
        for (int i = 0; i < 6; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#5")));
        for (int i = 0; i < 5; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#6")));
        for (int i = 0; i < 4; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#7")));
        for (int i = 0; i < 3; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#8")));
        for (int i = 0; i < 2; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#9")));
        for (int i = 0; i < 2; i++)
            tasks.Add(Task.Run(() => _sut.Capture("#10")));

        Task.WaitAll(tasks.ToArray());
        var hashtags = _sut.GetTopTenHashtags().ToArray();
        for (int i = 0; i < hashtags.Length - 1; i++)
        {
            Assert.True(hashtags[i].Entries >= hashtags[i + 1].Entries);
        }
    }

    [Fact]
    public void ShouldCalculateTweetRateInSeconds() {
        var _sut = new MetricService();
        _sut.Capture("");
        var rate = _sut.GetEntriesPerSecond();
        Assert.Equal(1, rate);
    }

    [Fact]
    public void ShouldTotalNumberOfTweetCapturedConcurrently() {
        var _sut = new MetricService();
        var tasks = new List<Task>(10);
        for (int i = 0; i < 10; i++)
            tasks.Add(Task.Run(() => _sut.Capture("")));

        Task.WaitAll(tasks.ToArray());
        Assert.Equal(10, _sut.GetTotalNumberOfEntriesReceived());
    }
}