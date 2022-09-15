namespace Challenge.Consumer.API.Events
{
    public interface IEvent
    {
    }
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleAsync(T @event, CancellationToken cancellationToken = default);
    }
}
