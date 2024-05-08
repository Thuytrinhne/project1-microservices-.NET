namespace Ordering.Domain.Abstractions
{
    public interface IAggregate<T>: IAggregate, IEntity<T>
    {

    }    
    public  interface IAggregate : IEntity // this is the special kind of entity that can manage the domain event 
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        IDomainEvent[] ClearDomainEvents(); 
    }
}
