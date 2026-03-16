using ArchLens.SharedKernel.Domain;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Domain;

public class ValueObjectTests
{
    private sealed class Address : ValueObject
    {
        public string Street { get; }
        public string City { get; }

        public Address(string street, string city)
        {
            Street = street;
            City = city;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
        }
    }

    private sealed class Money : ValueObject
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }

    private sealed class NullableVo : ValueObject
    {
        public string? Value { get; }

        public NullableVo(string? value) => Value = value;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    [Fact]
    public void Equals_SameValues_ShouldBeTrue()
    {
        var address1 = new Address("Main St", "NYC");
        var address2 = new Address("Main St", "NYC");

        address1.Equals(address2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentValues_ShouldBeFalse()
    {
        var address1 = new Address("Main St", "NYC");
        var address2 = new Address("Elm St", "NYC");

        address1.Equals(address2).Should().BeFalse();
    }

    [Fact]
    public void Equals_Null_ShouldBeFalse()
    {
        var address = new Address("Main St", "NYC");

        address.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentType_ShouldBeFalse()
    {
        var address = new Address("Main St", "NYC");
        var money = new Money(100, "USD");

        address.Equals(money).Should().BeFalse();
    }

    [Fact]
    public void Equals_NonValueObject_ShouldBeFalse()
    {
        var address = new Address("Main St", "NYC");

        address.Equals("not a value object").Should().BeFalse();
    }

    [Fact]
    public void Equals_IEquatable_SameValues_ShouldBeTrue()
    {
        var address1 = new Address("Main St", "NYC");
        var address2 = new Address("Main St", "NYC");

        ((IEquatable<ValueObject>)address1).Equals(address2).Should().BeTrue();
    }

    [Fact]
    public void Equals_IEquatable_Null_ShouldBeFalse()
    {
        var address = new Address("Main St", "NYC");

        address.Equals((ValueObject?)null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_SameValues_ShouldMatch()
    {
        var address1 = new Address("Main St", "NYC");
        var address2 = new Address("Main St", "NYC");

        address1.GetHashCode().Should().Be(address2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentValues_ShouldDiffer()
    {
        var address1 = new Address("Main St", "NYC");
        var address2 = new Address("Elm St", "LA");

        address1.GetHashCode().Should().NotBe(address2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithNullComponent_ShouldNotThrow()
    {
        var vo = new NullableVo(null);

        var act = () => vo.GetHashCode();
        act.Should().NotThrow();
    }

    [Fact]
    public void OperatorEquals_SameValues_ShouldBeTrue()
    {
        var address1 = new Address("Main St", "NYC");
        var address2 = new Address("Main St", "NYC");

        (address1 == address2).Should().BeTrue();
    }

    [Fact]
    public void OperatorNotEquals_DifferentValues_ShouldBeTrue()
    {
        var address1 = new Address("Main St", "NYC");
        var address2 = new Address("Elm St", "NYC");

        (address1 != address2).Should().BeTrue();
    }

    [Fact]
    public void OperatorEquals_BothNull_ShouldBeTrue()
    {
        Address? address1 = null;
        Address? address2 = null;

        (address1 == address2).Should().BeTrue();
    }

    [Fact]
    public void OperatorEquals_OneNull_ShouldBeFalse()
    {
        var address1 = new Address("Main St", "NYC");
        Address? address2 = null;

        (address1 == address2).Should().BeFalse();
    }

    [Fact]
    public void OperatorNotEquals_OneNull_ShouldBeTrue()
    {
        var address1 = new Address("Main St", "NYC");
        Address? address2 = null;

        (address1 != address2).Should().BeTrue();
    }
}
