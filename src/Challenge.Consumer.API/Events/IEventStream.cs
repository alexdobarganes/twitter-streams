using Challenge.Consumer.API.Events;

public interface IEventStream
{
    IEventStream Stream<T>() where T : class, IEvent;
}
