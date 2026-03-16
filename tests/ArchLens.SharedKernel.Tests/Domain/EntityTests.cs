using ArchLens.SharedKernel.Domain;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Domain;

public class EntityTests
{
    private sealed class TestEntity : Entity<Guid>
    {
        public TestEntity(Guid id) : base(id) { }
        public TestEntity() { }
    }

    [Fact]
    public void Constructor_WithId_ShouldSetId()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity(id);

        entity.Id.Should().Be(id);
    }

    [Fact]
    public void DefaultConstructor_ShouldSetDefaultId()
    {
        var entity = new TestEntity();

        entity.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void Equals_SameId_ShouldBeTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        entity1.Equals(entity2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentId_ShouldBeFalse()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        entity1.Equals(entity2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldBeFalse()
    {
        var entity = new TestEntity(Guid.NewGuid());

        entity.Equals((Entity<Guid>?)null).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNullObject_ShouldBeFalse()
    {
        var entity = new TestEntity(Guid.NewGuid());

        entity.Equals((object?)null).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNonEntityObject_ShouldBeFalse()
    {
        var entity = new TestEntity(Guid.NewGuid());

        entity.Equals("not an entity").Should().BeFalse();
    }

    [Fact]
    public void Equals_ObjectOverload_SameId_ShouldBeTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        object entity2 = new TestEntity(id);

        entity1.Equals(entity2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_SameId_ShouldMatch()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        entity1.GetHashCode().Should().Be(entity2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentId_ShouldDiffer()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
    }

    [Fact]
    public void OperatorEquals_SameId_ShouldBeTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        (entity1 == entity2).Should().BeTrue();
    }

    [Fact]
    public void OperatorNotEquals_DifferentId_ShouldBeTrue()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        (entity1 != entity2).Should().BeTrue();
    }

    [Fact]
    public void OperatorEquals_BothNull_ShouldBeTrue()
    {
        TestEntity? entity1 = null;
        TestEntity? entity2 = null;

        (entity1 == entity2).Should().BeTrue();
    }

    [Fact]
    public void OperatorEquals_OneNull_ShouldBeFalse()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        TestEntity? entity2 = null;

        (entity1 == entity2).Should().BeFalse();
    }

    [Fact]
    public void OperatorNotEquals_OneNull_ShouldBeTrue()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        TestEntity? entity2 = null;

        (entity1 != entity2).Should().BeTrue();
    }
}
