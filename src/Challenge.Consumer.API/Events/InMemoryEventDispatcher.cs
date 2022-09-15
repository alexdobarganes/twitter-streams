using Challenge.Consumer.API.Events;
using System.Collections.Concurrent;

internal sealed class InMemoryEventDispatcher : IEventDispatcher
{
    private readonly ConcurrentDictionary<Type, Type> _handlersTypes = new();
    private readonly IServiceProvider _serviceProvider;

    public InMemoryEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        if (@event is null) throw new InvalidOperationException("Event cannot be null.");

        await using var scope = _serviceProvider.CreateAsyncScope();
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
        var tasks = handlers.Select(x => x.HandleAsync(@event, cancellationToken));
        await Task.WhenAll(tasks);
    }

    public async Task PublishAsync(IEvent @event, Type type, CancellationToken cancellationToken = default)
    {
        if (@event is null) throw new InvalidOperationException("Event cannot be null.");

        await using var scope = _serviceProvider.CreateAsyncScope();
        var handlerType = _handlersTypes.GetOrAdd(type, typeof(IEventHandler<>).MakeGenericType(type));
        if (handlerType is null) throw new InvalidOperationException("Event method cannot be null.");

        var handlers = scope.ServiceProvider.GetServices(handlerType);
        var tasks = handlers.Select(x => (Task)x
            ?.GetType()
            .GetMethod("HandleAsync")
            ?.Invoke(x, new object[] { @event, cancellationToken })!);

        await Task.WhenAll(tasks);
    }
}