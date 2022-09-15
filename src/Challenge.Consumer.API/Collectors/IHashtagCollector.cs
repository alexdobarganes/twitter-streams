namespace Challenge.Consumer.API.Collectors
{
    public interface IHashtagCollector
    {
        void Collect(string text);
        IReadOnlyList<string> GetCollectedHashtags();
        IReadOnlyDictionary<string, long> GetCollectedHashtagsWithEntries();
    }
}