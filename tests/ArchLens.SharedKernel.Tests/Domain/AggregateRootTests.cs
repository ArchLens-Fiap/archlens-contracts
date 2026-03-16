using ArchLens.SharedKernel.Domain;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Domain;

public class AggregateRootTests
{
    private sealed record TestDomainEvent(string Data) : DomainEvent;

    private sealed class TestAggregate : AggregateRoot<Guid>
    {
        public TestAggregate(Guid id) : base(id) { }
        public TestAggregate() { }

        public void DoSomething(string data)
        {
            RaiseDomainEvent(new TestDomainEvent(data));
        }
    }

    [Fact]
    public void Constructor_WithId_ShouldSetId()
    {
        var id = Guid.NewGuid();
        var aggregate = new TestAggregate(id);

        aggregate.Id.Should().Be(id);
    }

    [Fact]
    public void DefaultConstructor_ShouldWork()
    {
        var aggregate = new TestAggregate();

        aggregate.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void DomainEvents_Initially_ShouldBeEmpty()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void RaiseDomainEvent_ShouldAddEventToList()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.DoSomething("test");

        aggregate.DomainEvents.Should().HaveCount(1);
        aggregate.DomainEvents[0].Should().BeOfType<TestDomainEvent>();
        ((TestDomainEvent)aggregate.DomainEvents[0]).Data.Should().Be("test");
    }

    [Fact]
    public void RaiseDomainEvent_MultipleTimes_ShouldAddAllEvents()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.DoSomething("first");
        aggregate.DoSomething("second");
        aggregate.DoSomething("third");

        aggregate.DomainEvents.Should().HaveCount(3);
    }

    [Fact]
    public void PopDomainEvents_ShouldReturnAllEventsAndClearList()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());
        aggregate.DoSomething("event1");
        aggregate.DoSomething("event2");

        var events = aggregate.PopDomainEvents();

        events.Should().HaveCount(2);
        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void PopDomainEvents_WhenEmpty_ShouldReturnEmptyList()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        var events = aggregate.PopDomainEvents();

        events.Should().BeEmpty();
    }

    [Fact]
    public void PopDomainEvents_CalledTwice_SecondCallShouldReturnEmpty()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());
        aggregate.DoSomething("event");

        var firstPop = aggregate.PopDomainEvents();
        var secondPop = aggregate.PopDomainEvents();

        firstPop.Should().HaveCount(1);
        secondPop.Should().BeEmpty();
    }
}
