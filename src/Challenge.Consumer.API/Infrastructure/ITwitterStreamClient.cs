namespace Challenge.Consumer.API.Infrastructure
{
    public interface ITwitterStreamClient
    {
        Task StartStreamAsync<T>(Action<T> callback, CancellationToken cancellationToken = default);
    }
}