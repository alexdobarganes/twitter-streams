using Challenge.Consumer.API.Events;

public interface IEventDispatcher
{
    Task PublishAsync(IEvent @event, Type type, CancellationToken cancellationToken = default);
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent;
}
